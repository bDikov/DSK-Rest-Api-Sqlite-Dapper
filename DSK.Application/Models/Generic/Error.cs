namespace DSK.Application.Models.Generic;

public record Error(string Code, string Reason)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("NULL_VALUE_ERROR", "Null value was provided");

    protected Error(Error original)
    {
        Code = original.Code;
        Reason = original.Reason;

    }
}
