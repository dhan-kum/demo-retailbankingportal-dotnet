using Microsoft.EntityFrameworkCore;
using RetailBankingPortal.Entities;

namespace RetailBankingPortal.Data;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
    {
    }

    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Transfer> Transfers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BankAccount>().HasData(
            new BankAccount
            {
                Id = 1,
                AccountNumber = "008596512563",
                AccountName = "John Doe",
                Balance = 52000.0,
                Type = "Savings"
            },
            new BankAccount
            {
                Id = 2,
                AccountNumber = "008596558965",
                AccountName = "John Doe",
                Balance = 7500.0,
                Type = "Checking"
            }
        );
    }
}
