using DSK.Application.Models.Generic;

namespace DSK.Application.Models.Generic.GenericResult;

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;
    public TValue? Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("The value of a failed result can not be accessed.");
            }
            return _value;
        }
    }
    internal Result(TValue value, bool isSuccess, Error error)
            : base(isSuccess, error)
    {
        _value = value;

    }

}

