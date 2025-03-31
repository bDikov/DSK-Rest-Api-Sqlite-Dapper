using System.Text.Json.Serialization;

namespace DSK.Api.Contracts.Responses;

public class InvoiceResponse
{

    [JsonPropertyName("invoiceNumber")]
    public required string Number { get; set; }


    [JsonPropertyName("invoiceAmount")]
    public decimal Amount { get; set; }
}
