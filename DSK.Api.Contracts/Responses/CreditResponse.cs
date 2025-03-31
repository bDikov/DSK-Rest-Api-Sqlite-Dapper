
using System.Text.Json.Serialization;

namespace DSK.Api.Contracts.Responses;

public class CreditResponse
{
    [JsonPropertyName("creditNumber")]
    public required string Number { get; set; }

    [JsonPropertyName("clientName")]
    public required string Name { get; set; }

    [JsonPropertyName("requestedAmount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("creditRequestDate")]
    public DateTime RequestDate { get; set; }

    [JsonPropertyName("creditStatus")]
    public required string Status { get; set; }

    [JsonPropertyName("creditInvoices")]
    public IEnumerable<InvoiceResponse>? Invoices { get; set; }
}
