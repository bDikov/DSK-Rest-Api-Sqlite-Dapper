using AutoMapper;
using DSK.Api.Contracts.Responses;
using DSK.Application.Models.Generic;
using DSK.Application.Models.Generic.GenericResult;
using DSK.Application.UseCases.CreditProcessing.Commands;
using DSK.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DSK.Api.Controllers
{
    [ApiController]
    [Route("api/credit-information")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CreditController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreditController> _logger;
        private readonly ICreditService _creditService;
        private readonly ISender _sender;

        public CreditController(IMapper mapper, ILogger<CreditController> logger, ICreditService creditService,
            ISender sender)
        {
            _mapper = mapper;
            _logger = logger;
            _creditService = creditService;
            _sender = sender;
        }

        [HttpGet("credits")]
        public async Task<ActionResult<Result<IEnumerable<CreditResponse>>>> GetCredits(CancellationToken cancellationToken)
        {
            var command = new CreditCommand();

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                var response = _mapper.Map<IEnumerable<CreditResponse>>(result.Value);
                return Ok(Result.Success(response));
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [HttpGet("summary")]
        public async Task<ActionResult<Result<CreditSummaryResponse>>> GetSummary(CancellationToken cancellationToken)
        {
            var command = new CreditSummaryCommand();

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsSuccess)
            {

                var response = _mapper.Map<CreditSummaryResponse>(result.Value);
                var responseResult = Result.Success(response);
                return Ok(responseResult);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }
    }
}