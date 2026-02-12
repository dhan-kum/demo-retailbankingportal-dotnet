using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailBankingPortal.Entities;

public class Transfer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string SenderAccount { get; set; } = string.Empty;

    public string ReceiverAccount { get; set; } = string.Empty;

    public double Amount { get; set; }
}
