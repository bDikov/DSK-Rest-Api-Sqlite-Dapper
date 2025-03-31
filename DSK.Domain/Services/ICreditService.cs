using DSK.Domain.Entities;
using DSK.Domain.ValueObject;

namespace DSK.Domain.Services;

public interface ICreditService
{
    /// <summary>  
    /// Asynchronously retrieves a collection of credits from the repository.  
    /// </summary>  
    /// <param name="cancellationToken">A cancellation token to signal the operation to cancel if needed.</param>  
    /// <returns>A task that represents the asynchronous operation, containing an <see cref="IEnumerable{Credit}"/> of <see cref="Credit"/> objects.</returns>  
    Task<IEnumerable<Credit>> GetCreditsAsync(CancellationToken cancellationToken);

    /// <summary>  
    /// Asynchronously retrieves the summary of credits from the repository.  
    /// </summary>  
    /// <param name="cancellationToken">A cancellation token to signal the operation to cancel if needed.</param>  
    /// <returns>A task that represents the asynchronous operation, containing a <see cref="CreditSummary"/> object.</returns>  
    Task<CreditSummary> GetCreditSummaryAsync(CancellationToken cancellationToken);
}
