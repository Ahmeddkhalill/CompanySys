namespace CompanySys.Application.Features.Teams.Commands.RemoveTeamMember;

public class RemoveTeamMemberCommandHandler : IRequestHandler<RemoveTeamMemberCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public RemoveTeamMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var team = await _context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == request.TeamId, cancellationToken);

        if (team is null)
            return Result.Failure(TeamErrors.NotFound);

        var member = team.Members.FirstOrDefault(m => m.EngineerId == request.EngineerId);

        if (member is null)
            return Result.Failure(TeamErrors.MemberNotInTeam);

        team.Members.Remove(member);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}