using CompanySys.Domain.Entities;
using CompanySys.Domain.Enums;
using DomainTaskStatus = CompanySys.Domain.Enums.TaskStatus;

namespace CompanySys.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var team = await _context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == request.TeamId, cancellationToken);

        if (team is null)
            return Result.Failure<Guid>(TaskErrors.TeamNotFound);

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project is null)
            return Result.Failure<Guid>(TaskErrors.ProjectNotFound);

        if (project.Status != ProjectStatus.InProgress)
            return Result.Failure<Guid>(TaskErrors.ProjectNotActive);

        if (request.EngineerIds.Count > 0)
        {
            var teamMemberIds = team.Members.Select(m => m.EngineerId).ToHashSet();
            var notInTeam = request.EngineerIds.Any(id => !teamMemberIds.Contains(id));

            if (notInTeam)
                return Result.Failure<Guid>(TaskErrors.EngineerNotInTeam);
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Status = DomainTaskStatus.Pending,
            Progress = 0,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TeamId = request.TeamId,
            ProjectId = request.ProjectId
        };

        foreach (var engineerId in request.EngineerIds.Distinct())
        {
            task.Assignments.Add(new TaskAssignment { TaskId = task.Id, EngineerId = engineerId });
        }

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(task.Id);
    }
}