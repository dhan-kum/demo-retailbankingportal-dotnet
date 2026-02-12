using System.ComponentModel.DataAnnotations;

namespace RetailBankingPortal.DTOs;

public class TransferDTO
{
    [Required]
    [StringLength(12, MinimumLength = 12)]
    public string? FromAccount { get; set; }
    
    [Required]
    [StringLength(12, MinimumLength = 12)]
    public string? ToAccount { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
}
