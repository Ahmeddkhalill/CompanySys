using CompanySys.Domain.Common;

namespace CompanySys.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string UserId { get; set; } = null!;
}