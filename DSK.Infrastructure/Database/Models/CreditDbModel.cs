using DSK.Infrastructure.Database.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSK.Infrastructure.Database.Models;

[Table("Credits")]
public class CreditDbModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public required string Id { get; set; }

    public required string Number { get; set; }

    public required string Name { get; set; }

    public decimal Amount { get; set; }

    public DateTime RequestDate { get; set; }

    public CreditStatusType Status { get; set; }

    public ICollection<InvoiceDbModel>? Invoices { get; set; }
}
