namespace CompanySys.Application.Features.Projects.Queries.GetAllProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, Result<List<ProjectResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetProjectsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<ProjectResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Projects.AsNoTracking();

        if (_currentUserService.IsInRole("Customer"))
        {
            query = query.Where(p => p.CustomerId == _currentUserService.UserId);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(p => p.Status == request.Status.Value);
        }

        var projects = await query
            .Select(p => new ProjectResponse(p.Id, p.Name, p.Description, p.Status, p.CustomerId, p.Teams.Count))
            .ToListAsync(cancellationToken);

        return Result.Success(projects);
    }
}