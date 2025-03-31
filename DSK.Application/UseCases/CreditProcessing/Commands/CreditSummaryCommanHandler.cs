using AutoMapper;
using DSK.Application.Models.DTOs;
using DSK.Application.Models.Generic;
using DSK.Application.Models.Generic.GenericResult;
using DSK.Domain.Services;
using MediatR;

namespace DSK.Application.UseCases.CreditProcessing.Commands;

public class CreditSummaryCommanHandler : IRequestHandler<CreditSummaryCommand, Result<CreditSummaryDto>>
{
    private readonly IMapper _mapper;
    private readonly ICreditService _creditService;

    public CreditSummaryCommanHandler(
        IMapper mapper,
        ICreditService creditService)
    {
        _mapper = mapper;
        _creditService = creditService;
    }
    public async Task<Result<CreditSummaryDto>> Handle(CreditSummaryCommand request, CancellationToken cancellationToken)
    {
        var creditSummaryResult = await _creditService.GetCreditSummaryAsync(cancellationToken);
        var creditSummaryResultDto = _mapper.Map<CreditSummaryDto>(creditSummaryResult);

        return Result.Success(creditSummaryResultDto);
    }
}
