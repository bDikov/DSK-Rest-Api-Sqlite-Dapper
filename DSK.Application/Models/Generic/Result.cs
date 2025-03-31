using DSK.Application.Models.Generic.GenericResult;

namespace DSK.Application.Models.Generic;

public class Result
{
    public bool IsSuccess { get; set; }
    public bool IsFailer => !IsSuccess;
    public Error? Error { get; set; }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }

    public static Result<TValue> Failed<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }

    internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }
        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }
        IsSuccess = isSuccess;
        Error = error;
    }
}
