using AutoMapper;
using DSK.Domain.Entities;
using DSK.Domain.Repositories;
using DSK.Domain.Services;
using DSK.Domain.ValueObject;

namespace DSK.Infrastructure.Services;

public class CreditService : ICreditService
{
    private readonly ICreditRepository _creditRepository;
    private readonly IMapper _mapper;
    public CreditService(
        ICreditRepository creditRepository,
        IMapper mapper)
    {
        _creditRepository = creditRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Credit>> GetCreditsAsync(CancellationToken cancelationToken)
    {
        var result = await _creditRepository.GetCreditsQueryableAsync(cancelationToken);

        return result;
    }

    public async Task<CreditSummary> GetCreditSummaryAsync(CancellationToken cancellationToken)
    {
        return await _creditRepository.GetCreditSummaryAsync(cancellationToken);
    }
}
