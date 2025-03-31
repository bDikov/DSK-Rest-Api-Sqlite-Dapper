using DSK.Domain.Entities;
using DSK.Domain.ValueObject;

namespace DSK.Domain.Services;

public interface ICreditService
{
    Task<IEnumerable<Credit>> GetCreditsAsync(CancellationToken cancelationToken);
    Task<CreditSummary> GetCreditSummaryAsync(CancellationToken cancellationToken);
}
