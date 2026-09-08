using CompanySys.Domain.Entities;

namespace CompanySys.Application.Features.Projects.Commands.AssignTeamToProject;

public class AssignTeamToProjectCommandHandler : IRequestHandler<AssignTeamToProjectCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public AssignTeamToProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(AssignTeamToProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project is null)
            return Result.Failure(ProjectErrors.NotFound);

        var teamExists = await _context.Teams.AnyAsync(t => t.Id == request.TeamId, cancellationToken);

        if (!teamExists)
            return Result.Failure(ProjectErrors.TeamNotFound);

        if (project.Teams.Any(pt => pt.TeamId == request.TeamId))
            return Result.Failure(ProjectErrors.TeamAlreadyAssigned);

        project.Teams.Add(new ProjectTeam { ProjectId = project.Id, TeamId = request.TeamId });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}