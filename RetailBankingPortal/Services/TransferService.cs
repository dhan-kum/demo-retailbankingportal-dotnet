using Microsoft.EntityFrameworkCore;
using RetailBankingPortal.Data;
using RetailBankingPortal.DTOs;
using RetailBankingPortal.Entities;

namespace RetailBankingPortal.Services;

public class TransferService
{
    private readonly BankingDbContext _context;
    private readonly ILogger<TransferService> _logger;

    public TransferService(BankingDbContext context, ILogger<TransferService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> TransferFunds(TransferDTO transferDto)
    {
        try
        {
            var fromAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.AccountNumber == transferDto.FromAccount);
            var toAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.AccountNumber == transferDto.ToAccount);

            if (fromAccount == null || toAccount == null)
            {
                _logger.LogWarning("Account not found");
                return false;
            }

            if (fromAccount.Balance < transferDto.Amount)
            {
                _logger.LogWarning("Insufficient balance");
                return false;
            }

            fromAccount.Balance -= transferDto.Amount;
            toAccount.Balance += transferDto.Amount;

            var transfer = new Transfer
            {
                FromAccount = transferDto.FromAccount,
                ToAccount = transferDto.ToAccount,
                Amount = transferDto.Amount,
                TransferDate = DateTime.UtcNow
            };

            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Transfer successful: {Amount} from {From} to {To}",
                transferDto.Amount, transferDto.FromAccount, transferDto.ToAccount);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transfer");
            return false;
        }
    }
}
