using DSK.Domain.Entities;
using DSK.Domain.ValueObject;

namespace DSK.Domain.Repositories;

public interface ICreditRepository
{
    Task<IEnumerable<Credit>> GetCreditsQueryableAsync(CancellationToken cancellationToken);

    Task<CreditSummary> GetCreditSummaryAsync(CancellationToken cancellationToken);
}
