namespace CompanySys.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }

    public string? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string EntityName { get; set; } = null!;

    public string? EntityId { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Details { get; set; }
}