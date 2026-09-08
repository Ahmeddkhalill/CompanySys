namespace CompanySys.Domain.Abstractions;

public static class RegexPatterns
{
    public const string Password = @"(?=(.*[0-9]))(?=.*[\!@#$%^&*()\[\]{}\-_+=~`|:;""'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
}
public static class SeedIds
{
    public static readonly Guid DefaultCompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111");
}