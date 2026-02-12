namespace RetailBankingPortal.DTOs;

public class TransferDTO
{
    public string SenderAccount { get; set; } = string.Empty;

    public string ReceiverAccount { get; set; } = string.Empty;

    public double Amount { get; set; }
}
