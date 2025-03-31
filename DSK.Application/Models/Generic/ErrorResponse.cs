namespace DSK.Application.Models.Generic;

public class ErrorResponse
{
    public required string ErrorCode { get; set; }
    public required string ErrorReason { get; set; }
}
