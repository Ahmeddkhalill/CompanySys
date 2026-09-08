namespace CompanySys.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDetailsResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetProjectByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProjectDetailsResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .Select(p => new ProjectDetailsResponse(
                p.Id, p.Name, p.Description, p.Status, p.CustomerId,
                p.Teams.Select(pt => new ProjectTeamDto(pt.Team.Id, pt.Team.Name)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (project is null)
            return Result.Failure<ProjectDetailsResponse>(ProjectErrors.NotFound);

        return Result.Success(project);
    }
}