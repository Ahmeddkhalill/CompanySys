namespace CompanySys.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UpdateTeamCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Result> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (team is null)
            return Result.Failure(TeamErrors.NotFound);

        var teamLeadExists = await _identityService.UserExistsAsync(request.TeamLeadId, cancellationToken);

        if (!teamLeadExists)
            return Result.Failure(TeamErrors.TeamLeadNotFound);

        var nameExists = await _context.Teams
            .AnyAsync(t => t.DepartmentId == team.DepartmentId
                        && t.Name.ToLower() == request.Name.ToLower()
                        && t.Id != request.Id, cancellationToken);

        if (nameExists)
            return Result.Failure(TeamErrors.AlreadyExists);

        team.Name = request.Name;
        team.TeamLeadId = request.TeamLeadId;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}