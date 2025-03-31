using AutoMapper;
using Dapper;
using DSK.Domain.Entities;
using DSK.Domain.Repositories;
using DSK.Domain.ValueObject;
using DSK.Infrastructure.Database.Enums;
using DSK.Infrastructure.Database.Models;
using DSK.Infrastructure.Interfaces.DbHelpers;

namespace DSK.Infrastructure.Repositories;

public class CreditRepository : ICreditRepository
{
    private readonly IDbHelper _dbHelper;
    private readonly IMapper _mapper;
    public CreditRepository(
        IDbHelper dbHelper,
        IMapper mapper)
    {
        _dbHelper = dbHelper;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Credit>> GetCreditsQueryableAsync(CancellationToken cancellationToken)
    {
        using var connection = await _dbHelper.GetInMemoryDbConnectionAsync(cancellationToken);

        var sql = @"  
        SELECT c.*, i.*  
        FROM Credits c  
        LEFT JOIN Invoices i ON c.Id = i.CreditId";

        var creditInvoices = new Dictionary<string, CreditDbModel>();

        var result = connection.Query<CreditDbModel, InvoiceDbModel, CreditDbModel>(
             sql,
             (credit, invoice) =>
             {
                 if (!creditInvoices.TryGetValue(credit.Id, out CreditDbModel? value))
                 {
                     value = credit;
                     creditInvoices[credit.Id] = value;
                     credit.Invoices = [];
                 }

                 if (invoice != null)
                 {
                     value.Invoices!.Add(invoice);
                 }

                 return value;
             },
             splitOn: "Id"
         ).ToList();

        return _mapper.Map<List<Credit>>(creditInvoices.Values.ToList());
    }

    public async Task<CreditSummary> GetCreditSummaryAsync(CancellationToken cancellationToken)
    {
        using var connection = await _dbHelper.GetInMemoryDbConnectionAsync(cancellationToken);

        var result = await connection.QuerySingleOrDefaultAsync<dynamic>(
            @"SELECT   
            SUM(CASE WHEN Status = @Paid THEN Amount ELSE 0 END) AS TotalPaid,  
            SUM(CASE WHEN Status = @AwaitingPayment THEN Amount ELSE 0 END) AS TotalAwaitingPayment  
        FROM Credits", new { Paid = (int)CreditStatusType.Paid, AwaitingPayment = (int)CreditStatusType.AwaitingPayment });

        var totalPaid = result?.TotalPaid != null ? (decimal)result.TotalPaid : 0m;
        var totalAwaitingPayment = result?.TotalAwaitingPayment != null ? (decimal)result.TotalAwaitingPayment : 0m;
        var totalSum = totalPaid + totalAwaitingPayment;

        var paidPercentage = totalSum > 0 ? Math.Round((totalPaid / totalSum) * 100, 2) : 0;
        var awaitingPaymentPercentage = totalSum > 0 ? Math.Round((totalAwaitingPayment / totalSum) * 100, 2) : 0;

        var output = new CreditSummary()
        {
            TotalPaidSum = totalPaid,
            FullAwaitingPaymentSum = totalAwaitingPayment,
            TotalPaidSumPercentage = paidPercentage,
            TotalAwaitingPaymentPercentage = awaitingPaymentPercentage,
            FullAwaitingPaymentSumPercentage = totalSum,
        };

        return output;
    }
}
