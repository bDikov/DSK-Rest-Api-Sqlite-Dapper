namespace DSK.Application.Models.DTOs;

public class CreditSummaryDto
{
    public decimal TotalPaidSum { get; set; }

    public decimal FullAwaitingPaymentSum { get; set; }

    public decimal TotalPaidSumPercentage { get; set; }

    public decimal TotalAwaitingPaymentPercentage { get; set; }

    public decimal FullAwaitingPaymentSumPercentage { get; set; }
}
