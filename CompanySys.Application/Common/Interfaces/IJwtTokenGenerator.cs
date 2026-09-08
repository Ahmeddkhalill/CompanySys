using System.Security.Claims;

namespace CompanySys.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    Task<(string Token, int ExpiresIn)> GenerateTokenAsync(
        string userId,
        string email,
        string firstName,
        string lastName,
        IList<string> roles,
        IEnumerable<Claim>? permissions = null);

    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}