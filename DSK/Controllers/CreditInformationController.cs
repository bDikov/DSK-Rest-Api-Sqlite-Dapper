using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DSK.Api.Controllers;

[ApiController]

[Route("api/v{version:apiVersion}/credit-information")]

[ApiConventionType(typeof(DefaultApiConventions))]
public class CreditInformationController : BaseController
{

    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly ILogger<CreditInformationController> _logger;
    public CreditInformationController(
        IHttpContextAccessor contextAccessor,
        ISender sender,
        IMapper mapper,
        ILogger<CreditInformationController> logger)
        : base(contextAccessor)
    {
        _sender = sender;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpGet("credits")]
    public async Task<IActionResult> GetCreditsWithInvoices()
    {
        //var credits = await _context.Credits
        //    .Include(c => c.Invoices)
        //    .ToListAsync();

        //return Ok(credits);
        throw new NotImplementedException();
    }

    [HttpGet("status-summary")]
    public async Task<IActionResult> GetCreditStatusSummary()
    {
        //var credits = await _context.Credits
        //    .Where(c => c.Status == "Paid" || c.Status == "AwaitingPayment")
        //    .ToListAsync();

        //var totalCredits = credits.Sum(c => c.Invoices.Sum(i => i.Amount));
        //var totalPaid = credits.Where(c => c.Status == "Paid").Sum(c => c.Invoices.Sum(i => i.Amount));
        //var totalAwaitingPayment = credits.Where(c => c.Status == "AwaitingPayment").Sum(c => c.Invoices.Sum(i => i.Amount));

        //var paidPercentage = totalCredits > 0 ? (totalPaid / totalCredits) * 100 : 0;
        //var awaitingPaymentPercentage = totalCredits > 0 ? (totalAwaitingPayment / totalCredits) * 100 : 0;

        //var summary = new
        //{
        //    TotalPaid = totalPaid,
        //    TotalAwaitingPayment = totalAwaitingPayment,
        //    PaidPercentage = paidPercentage,
        //    AwaitingPaymentPercentage = awaitingPaymentPercentage
        //};

        //return Ok(summary);

        throw new NotImplementedException();
    }

}
