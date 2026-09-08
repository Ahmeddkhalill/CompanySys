using CompanySys.Domain.Entities;

namespace CompanySys.Application.Features.Teams.Commands.AddTeamMember;

public class AddTeamMemberCommandHandler : IRequestHandler<AddTeamMemberCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public AddTeamMemberCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Result> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var team = await _context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == request.TeamId, cancellationToken);

        if (team is null)
            return Result.Failure(TeamErrors.NotFound);

        var engineerExists = await _identityService.UserExistsAsync(request.EngineerId, cancellationToken);

        if (!engineerExists)
            return Result.Failure(TeamErrors.MemberNotFound);

        if (team.Members.Any(m => m.EngineerId == request.EngineerId))
            return Result.Failure(TeamErrors.MemberAlreadyInTeam);

        team.Members.Add(new TeamMember { TeamId = team.Id, EngineerId = request.EngineerId });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}