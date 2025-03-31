using AutoMapper;
using DSK.Application.Models.DTOs;
using DSK.Application.Models.Generic;
using DSK.Application.Models.Generic.GenericResult;
using DSK.Domain.Services;
using MediatR;

namespace DSK.Application.UseCases.CreditProcessing.Commands
{
    public class CreditCommandHandler : IRequestHandler<CreditCommand, Result<IEnumerable<CreditDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ICreditService _creditService;

        public CreditCommandHandler(
            IMapper mapper,
            ICreditService creditService)
        {
            _mapper = mapper;
            _creditService = creditService;
        }
        public async Task<Result<IEnumerable<CreditDto>>> Handle(CreditCommand request, CancellationToken cancellationToken)
        {
            var creditResult = await _creditService.GetCreditsAsync(cancellationToken);

            var creditResultDto = _mapper.Map<IEnumerable<CreditDto>>(creditResult);
            return Result.Success(creditResultDto);
        }
    }
}