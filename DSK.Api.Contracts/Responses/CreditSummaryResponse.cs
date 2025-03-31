namespace DSK.Api.Contracts.Responses;

public class CreditSummaryResponse
{
    public decimal TotalPaidSum { get; set; }

    public decimal FullAwaitingPaymentSum { get; set; }

    public decimal TotalPaidSumPercentage { get; set; }

    public decimal TotalAwaitingPaymentPercentage { get; set; }

    public decimal FullAwaitingPaymentSumPercentage { get; set; }
}