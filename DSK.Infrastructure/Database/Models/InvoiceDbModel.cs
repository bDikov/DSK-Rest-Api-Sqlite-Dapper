using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSK.Infrastructure.Database.Models;

[Table("Invoices")]
public class InvoiceDbModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public required string Id { get; set; }

    public required string CreditId { get; set; }

    public DateTime CreatedOn { get; set; }

    public required string Number { get; set; }

    public decimal Amount { get; set; }
}
