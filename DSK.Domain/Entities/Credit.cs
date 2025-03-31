using DSK.Domain.Guards;

namespace DSK.Domain.Entities;

public class Credit
{
    public string Number { get; } = null!;

    public string Name { get; } = null!;

    public decimal Amount { get; }

    public DateTime RequestDate { get; }

    public string Status { get; } = null!;

    public List<Invoice>? Invoices { get; }

    public Credit
        (string number,
         string name,
         decimal amount,
         DateTime requestDate,
         string status,
         List<Invoice>? invoices = null)
    {
        // EntitiesValidator.ValidateInvoiceNumberFormat(number);
        EntitiesValidator.ValidateAmount(amount);

        Number = number;
        Name = name;
        Amount = amount;
        RequestDate = requestDate;
        Status = status;
        Invoices = invoices;
    }
}
