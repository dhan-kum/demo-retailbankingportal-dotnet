using Microsoft.EntityFrameworkCore;
using RetailBankingPortal.Data;
using RetailBankingPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add DbContext with InMemory database
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseInMemoryDatabase("BankingDb"));

// Register services
builder.Services.AddScoped<TransferService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddSingleton<ConsumerService>();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
