namespace CompanySys.Application.Features.AuditLogs.Queries.GetAuditLogs;

public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, Result<List<AuditLogResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetAuditLogsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<AuditLogResponse>>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.AuditLogs.AsNoTracking().OrderByDescending(a => a.Timestamp).AsQueryable();

        if (!string.IsNullOrEmpty(request.UserId))
            query = query.Where(a => a.UserId == request.UserId);

        if (!string.IsNullOrEmpty(request.Action))
            query = query.Where(a => a.Action == request.Action);

        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

        var logs = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AuditLogResponse(a.Id, a.UserId, a.Action, a.EntityName, a.EntityId, a.Timestamp, a.Details))
            .ToListAsync(cancellationToken);

        return Result.Success(logs);
    }
}