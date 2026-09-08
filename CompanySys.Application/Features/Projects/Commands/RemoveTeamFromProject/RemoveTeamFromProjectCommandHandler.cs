namespace CompanySys.Application.Features.Projects.Commands.RemoveTeamFromProject;

public class RemoveTeamFromProjectCommandHandler : IRequestHandler<RemoveTeamFromProjectCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public RemoveTeamFromProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(RemoveTeamFromProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project is null)
            return Result.Failure(ProjectErrors.NotFound);

        var projectTeam = project.Teams.FirstOrDefault(pt => pt.TeamId == request.TeamId);

        if (projectTeam is null)
            return Result.Failure(ProjectErrors.TeamNotAssigned);

        project.Teams.Remove(projectTeam);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}