using RetailBankingPortal.Data;
using RetailBankingPortal.Entities;

namespace RetailBankingPortal.Services;

public class TransferService
{
    private readonly BankingDbContext _dbContext;
    private readonly ILogger<TransferService> _logger;

    public TransferService(BankingDbContext dbContext, ILogger<TransferService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public Dictionary<string, BankAccount?>? ExecuteTransfer(Transfer transfer)
    {
        try
        {
            var senderAccount = _dbContext.BankAccounts
                .FirstOrDefault(a => a.AccountNumber == transfer.SenderAccount);
            var receiverAccount = _dbContext.BankAccounts
                .FirstOrDefault(a => a.AccountNumber == transfer.ReceiverAccount);

            if (senderAccount != null && receiverAccount != null)
            {
                if (senderAccount.Balance > transfer.Amount)
                {
                    senderAccount.Balance -= transfer.Amount;
                    receiverAccount.Balance += transfer.Amount;
                    _dbContext.BankAccounts.Update(senderAccount);
                    _dbContext.BankAccounts.Update(receiverAccount);
                    _dbContext.SaveChanges();

                    return new Dictionary<string, BankAccount?>
                    {
                        { "senderAccount", senderAccount },
                        { "receiverAccount", receiverAccount }
                    };
                }
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("ErrorMsg: Account number is incorrect. StackTrace: {StackTrace}", exc.StackTrace);
            throw;
        }

        return null;
    }
}
