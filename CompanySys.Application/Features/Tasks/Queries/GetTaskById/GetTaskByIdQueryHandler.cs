namespace CompanySys.Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskDetailsResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetTaskByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TaskDetailsResponse>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .AsNoTracking()
            .Where(t => t.Id == request.Id)
            .Select(t => new TaskDetailsResponse(
                t.Id, t.Title, t.Description, t.Priority, t.Status, t.Progress,
                t.StartDate, t.EndDate, t.TeamId, t.ProjectId,
                t.Assignments.Select(a => a.EngineerId).ToList(),
                t.Dependencies.Select(d => new TaskDependencyDto(
                    d.DependsOnTask.Id, d.DependsOnTask.Title, d.DependsOnTask.Status)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
            return Result.Failure<TaskDetailsResponse>(TaskErrors.NotFound);

        return Result.Success(task);
    }
}