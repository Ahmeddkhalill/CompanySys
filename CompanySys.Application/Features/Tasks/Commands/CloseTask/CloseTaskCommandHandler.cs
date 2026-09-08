using DomainTaskStatus = CompanySys.Domain.Enums.TaskStatus;

namespace CompanySys.Application.Features.Tasks.Commands.CloseTask;

public class CloseTaskCommandHandler : IRequestHandler<CloseTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CloseTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CloseTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            return Result.Failure(TaskErrors.NotFound);

        if (task.Status != DomainTaskStatus.Completed)
            return Result.Failure(TaskErrors.InvalidStatusForAction);

        task.Status = DomainTaskStatus.Closed;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}