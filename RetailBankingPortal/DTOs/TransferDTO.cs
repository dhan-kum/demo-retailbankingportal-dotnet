namespace RetailBankingPortal.DTOs;

public class TransferDTO
{
    public string? FromAccount { get; set; }
    
    public string? ToAccount { get; set; }
    
    public decimal Amount { get; set; }
}
