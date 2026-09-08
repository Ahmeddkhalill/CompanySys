namespace CompanySys.Application.Features.AuditLogs.Queries.GetAuditLogs;

public record AuditLogResponse(Guid Id, string? UserId, string Action, string EntityName, string? EntityId, DateTime Timestamp, string? Details);

public record GetAuditLogsQuery(string? UserId, string? Action, int Page, int PageSize) : IRequest<Result<List<AuditLogResponse>>>;