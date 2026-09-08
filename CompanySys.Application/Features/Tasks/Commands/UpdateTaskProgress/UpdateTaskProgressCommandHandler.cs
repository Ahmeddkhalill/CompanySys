using DomainTaskStatus = CompanySys.Domain.Enums.TaskStatus;

namespace CompanySys.Application.Features.Tasks.Commands.UpdateTaskProgress;

public class UpdateTaskProgressCommandHandler : IRequestHandler<UpdateTaskProgressCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTaskProgressCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(UpdateTaskProgressCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .Include(t => t.Assignments)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            return Result.Failure(TaskErrors.NotFound);

        var isAssigned = task.Assignments.Any(a => a.EngineerId == _currentUserService.UserId);

        if (!isAssigned)
            return Result.Failure(TaskErrors.NotAssignedToYou);

        task.Progress = request.Progress;

        if (task.Status == DomainTaskStatus.Pending)
            task.Status = DomainTaskStatus.InProgress;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}