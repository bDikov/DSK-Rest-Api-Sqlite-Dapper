using DSK.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace DSK.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;

    public BaseController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    internal BadRequestObjectResult BadRequest(Error error)
    {
        return BadRequest(GetErrorResponse(error, _contextAccessor.HttpContext!));
    }

    internal UnauthorizedObjectResult Unauthorized(Error error)
    {
        return Unauthorized(GetErrorResponse(error, _contextAccessor.HttpContext!));
    }

    private ErrorResponse GetErrorResponse(Error error, HttpContext context)
    {
        return new ErrorResponse()
        {
            ErrorCode = error.Code,
            ErrorReason = error.Reason,
        };
    }
}