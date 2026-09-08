namespace CompanySys.Application.Features.Tasks.Queries.GetAllTasks;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Result<List<TaskResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTasksQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<TaskResponse>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tasks.AsNoTracking();

        if (_currentUserService.IsInRole("Engineer") && !_currentUserService.IsInRole("TeamLead"))
        {
            query = query.Where(t => t.Assignments.Any(a => a.EngineerId == _currentUserService.UserId));
        }

        if (request.TeamId.HasValue)
            query = query.Where(t => t.TeamId == request.TeamId.Value);

        if (request.ProjectId.HasValue)
            query = query.Where(t => t.ProjectId == request.ProjectId.Value);

        if (request.Status.HasValue)
            query = query.Where(t => t.Status == request.Status.Value);

        var tasks = await query
            .Select(t => new TaskResponse(
                t.Id, t.Title, t.Priority, t.Status, t.Progress, t.StartDate, t.EndDate,
                t.TeamId, t.ProjectId, t.Assignments.Select(a => a.EngineerId).ToList()))
            .ToListAsync(cancellationToken);

        return Result.Success(tasks);
    }
}