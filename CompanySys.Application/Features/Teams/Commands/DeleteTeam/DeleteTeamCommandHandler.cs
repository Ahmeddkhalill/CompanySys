namespace CompanySys.Application.Features.Teams.Commands.DeleteTeam;

public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteTeamCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _context.Teams
            .Include(t => t.Projects)
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (team is null)
            return Result.Failure(TeamErrors.NotFound);

        if (team.Projects.Any() || team.Tasks.Any())
            return Result.Failure(TeamErrors.HasAssociatedWork);

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}