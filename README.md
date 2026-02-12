# Eviden Retail Banking Portal - .NET Edition

This is a .NET 8.0 migration of the [Eviden Starter Kit for Spring Application](https://github.com/dhan-kum/demo-retailbankingportal-baseapp), originally built with Java Spring Boot + React.js.

## Technology Stack

| Component | Original (Java) | Migrated (.NET) |
|-----------|-----------------|-----------------|
| Backend Framework | Spring Boot 2.2.4 | ASP.NET Core 8.0 |
| Language | Java 11 | C# 12 |
| ORM | Spring Data JPA | Entity Framework Core 8.0 |
| Database | H2 (In-Memory) | EF Core InMemory |
| Message Queue | RabbitMQ (AMQP) | RabbitMQ.Client |
| Frontend | React.js (webpack) | React.js (static bundle) |
| Build Tool | Maven | dotnet CLI |
| Container | Docker (OpenJDK 21) | Docker (.NET 8.0) |

## Project Structure

```
RetailBankingPortal/
├── Controllers/
│   ├── BankAccountController.cs    # REST API endpoints (/api/bankaccounts)
│   └── HomeController.cs           # Serves index page (/)
├── Data/
│   └── BankingDbContext.cs          # EF Core DbContext with seed data
├── DTOs/
│   └── TransferDTO.cs              # Transfer request DTO
├── Entities/
│   ├── BankAccount.cs              # Bank account entity
│   └── Transfer.cs                 # Transfer entity
├── Services/
│   ├── TransferService.cs          # Transfer business logic
│   ├── FileService.cs              # ZIP file creation
│   └── ConsumerService.cs          # RabbitMQ message consumer
├── Utils/
│   └── Utility.cs                  # Helper methods
├── wwwroot/
│   ├── index.html                  # Main HTML page
│   ├── main.css                    # Styles
│   └── built/
│       └── bundle.js               # React frontend bundle
├── Program.cs                      # Application entry point
├── appsettings.json                # Configuration
└── RetailBankingPortal.csproj      # Project file
```

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | `/` | Serves the main HTML page |
| GET | `/api/bankaccounts` | Get all bank accounts |
| GET | `/api/bankaccounts/{id}` | Get bank account by account number |
| POST | `/api/bankaccounts/transfer` | Transfer funds between accounts |
| POST | `/api/bankaccounts/logmessage?logmsg={msg}` | Log a message |
| GET | `/api/bankaccounts/createzip?sourceDir={dir}&zipFile={file}` | Create ZIP archive |
| GET | `/api/bankaccounts/connect` | Connect to RabbitMQ queue |

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- (Optional) Docker

### Run Locally

```bash
cd RetailBankingPortal
dotnet restore
dotnet run
```

The application will start on `http://localhost:5000` (or the configured port).

### Run with Docker

```bash
docker build -t retailbankingportal .
docker run -p 8080:8080 retailbankingportal
```

### Seed Data

On startup, the application seeds two bank accounts:

| Account Number | Account Name | Balance | Type |
|---------------|-------------|---------|------|
| 008596512563 | John Doe | $52,000.00 | Savings |
| 008596558965 | John Doe | $7,500.00 | Checking |

## Migration Notes

- **Spring Data JPA** -> **Entity Framework Core**: JPA annotations replaced with EF Core data annotations and fluent API configuration.
- **Spring DI (@Autowired)** -> **.NET DI (constructor injection)**: Services registered in `Program.cs` using built-in DI container.
- **H2 In-Memory DB** -> **EF Core InMemory Provider**: Same in-memory database pattern for development.
- **Spring Data REST** -> **ASP.NET Core Controllers**: Explicit controller endpoints instead of auto-generated REST endpoints.
- **Thymeleaf** -> **Static Files**: HTML served from `wwwroot/` using static file middleware.
- **Logback** -> **Microsoft.Extensions.Logging**: Built-in .NET logging framework replaces Logback + custom Elasticsearch appender.
- **Maven** -> **dotnet CLI**: Build system migrated to .NET CLI with `.csproj` project files.
