using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RetailBankingPortal.Services;

public class ConsumerService
{
    private readonly ILogger<ConsumerService> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
    }

    public bool ConnectToQueue(string hostName = "localhost", string queueName = "banking-queue")
    {
        try
        {
            var factory = new ConnectionFactory { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message: {Message}", message);
            };

            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            _logger.LogInformation("Connected to RabbitMQ queue: {QueueName}", queueName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error connecting to RabbitMQ");
            return false;
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
