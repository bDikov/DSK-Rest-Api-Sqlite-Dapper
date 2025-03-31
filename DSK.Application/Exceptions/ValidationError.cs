namespace DSK.Application.Exceptions;

public sealed record ValidationError(string PropertyName, string ErrorMessage)
{
    public override string ToString() => $"'$.{PropertyName}' - {ErrorMessage}";
}

