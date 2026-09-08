using CompanySys.Domain.Entities;

namespace CompanySys.Application.Features.Tasks.Commands.AssignEngineer;

public class AssignEngineerCommandHandler : IRequestHandler<AssignEngineerCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public AssignEngineerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(AssignEngineerCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .Include(t => t.Assignments)
            .Include(t => t.Team).ThenInclude(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            return Result.Failure(TaskErrors.NotFound);

        var isInTeam = task.Team.Members.Any(m => m.EngineerId == request.EngineerId);

        if (!isInTeam)
            return Result.Failure(TaskErrors.EngineerNotInTeam);

        if (task.Assignments.Any(a => a.EngineerId == request.EngineerId))
            return Result.Failure(TaskErrors.EngineerAlreadyAssigned);

        task.Assignments.Add(new TaskAssignment { TaskId = task.Id, EngineerId = request.EngineerId });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}