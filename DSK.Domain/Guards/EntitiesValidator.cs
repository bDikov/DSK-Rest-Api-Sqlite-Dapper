using System.Text.RegularExpressions;

namespace DSK.Domain.Guards
{
    public static class EntitiesValidator
    {
        public static void ValidateAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
            }
        }

        public static void ValidateInvoiceNumberFormat(string? invoiceNumber)
        {
            if (string.IsNullOrEmpty(invoiceNumber))
            {
                return;
            }

            // Define the regex pattern to match "I-XX-YY" format  
            // Assuming XX can be any characters and YY is a two-digit number  
            var pattern = @"^I-\w{1,}-\d{2}$";

            if (!Regex.IsMatch(invoiceNumber, pattern))
            {
                throw new FormatException($"The invoice number '{invoiceNumber}' is not in the correct format.");
            }
        }
    }
}
