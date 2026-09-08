namespace CompanySys.Domain.Abstractions;

public record Error(string Code, string Description, int? statusCode = null)
{
    public static readonly Error None = new(string.Empty, string.Empty, null);
}