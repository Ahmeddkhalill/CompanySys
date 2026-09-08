namespace CompanySys.Application.Features.Tasks.Commands.RemoveEngineer;

public class RemoveEngineerCommandHandler : IRequestHandler<RemoveEngineerCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public RemoveEngineerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(RemoveEngineerCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.Include(t => t.Assignments).FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            return Result.Failure(TaskErrors.NotFound);

        var assignment = task.Assignments.FirstOrDefault(a => a.EngineerId == request.EngineerId);

        if (assignment is null)
            return Result.Failure(TaskErrors.EngineerNotAssigned);

        task.Assignments.Remove(assignment);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}