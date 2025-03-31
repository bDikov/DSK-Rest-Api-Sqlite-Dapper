using DSK.Domain.Guards;

namespace DSK.Domain.Entities;

public class Invoice
{
    public string? Number { get; }

    public decimal Amount { get; }

    public Invoice(string? number, decimal amount)
    {
        // EntitiesValidator.ValidateInvoiceNumberFormat(number);
        EntitiesValidator.ValidateAmount(amount);

        Number = number;
        Amount = amount;
    }

}
