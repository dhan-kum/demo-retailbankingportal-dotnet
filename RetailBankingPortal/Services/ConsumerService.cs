using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RetailBankingPortal.Services;

public class ConsumerService
{
    private readonly ILogger<ConsumerService> _logger;

    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
    }

    public void Connect(string queueName)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Received message: {Message}", message);
        };

        _logger.LogInformation("Consumer {Consumer}", consumer);
    }
}
