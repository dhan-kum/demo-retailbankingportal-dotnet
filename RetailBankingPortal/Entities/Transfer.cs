using System.ComponentModel.DataAnnotations;

namespace RetailBankingPortal.Entities;

public class Transfer
{
    [Key]
    public int Id { get; set; }
    
    public string? FromAccount { get; set; }
    
    public string? ToAccount { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateTime TransferDate { get; set; }
}
