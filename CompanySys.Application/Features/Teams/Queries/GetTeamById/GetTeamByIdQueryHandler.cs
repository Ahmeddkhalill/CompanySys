namespace CompanySys.Application.Features.Teams.Queries.GetTeamById;

public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, Result<TeamDetailsResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetTeamByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TeamDetailsResponse>> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = await _context.Teams
            .AsNoTracking()
            .Where(t => t.Id == request.Id)
            .Select(t => new TeamDetailsResponse(
                t.Id,
                t.Name,
                t.DepartmentId,
                t.Department.Name,
                t.TeamLeadId,
                t.Members.Select(m => m.EngineerId).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (team is null)
            return Result.Failure<TeamDetailsResponse>(TeamErrors.NotFound);

        return Result.Success(team);
    }
}