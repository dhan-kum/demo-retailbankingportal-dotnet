using System.ComponentModel.DataAnnotations;

namespace RetailBankingPortal.Entities;

public class BankAccount
{
    [Key]
    public string? AccountNumber { get; set; }
    
    public string? AccountName { get; set; }
    
    public string? AccountType { get; set; }
    
    public decimal Balance { get; set; }
}
