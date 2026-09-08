namespace CompanySys.Application.Features.Tasks.Commands.RemoveTaskDependency;

public class RemoveTaskDependencyCommandHandler : IRequestHandler<RemoveTaskDependencyCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public RemoveTaskDependencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(RemoveTaskDependencyCommand request, CancellationToken cancellationToken)
    {
        var dependency = await _context.TaskDependencies
            .FirstOrDefaultAsync(d => d.TaskId == request.TaskId && d.DependsOnTaskId == request.DependsOnTaskId, cancellationToken);

        if (dependency is null)
            return Result.Failure(TaskErrors.DependencyNotFound);

        _context.TaskDependencies.Remove(dependency);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}