namespace DSK.Application.Models.DTOs;

public class CreditDto
{
    public string Number { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime RequestDate { get; set; }

    public string Status { get; set; } = null!;

    public List<InvoiceDto>? Invoices { get; set; }

}
