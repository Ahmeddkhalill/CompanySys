namespace CompanySys.Application.Features.Teams.Queries.GetAllTeams;

public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, Result<List<TeamResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetTeamsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TeamResponse>>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Teams.AsNoTracking();

        if (request.DepartmentId.HasValue)
            query = query.Where(t => t.DepartmentId == request.DepartmentId.Value);

        var teams = await query
            .Select(t => new TeamResponse(
                t.Id,
                t.Name,
                t.DepartmentId,
                t.Department.Name,
                t.TeamLeadId,
                t.Members.Count))
            .ToListAsync(cancellationToken);

        return Result.Success(teams);
    }
}