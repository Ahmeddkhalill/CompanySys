using DomainTaskStatus = CompanySys.Domain.Enums.TaskStatus;

namespace CompanySys.Application.Features.Tasks.Commands.SubmitTask;

public class SubmitTaskCommandHandler : IRequestHandler<SubmitTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SubmitTaskCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(SubmitTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .Include(t => t.Assignments)
            .Include(t => t.Dependencies)
                .ThenInclude(d => d.DependsOnTask)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            return Result.Failure(TaskErrors.NotFound);

        var isAssigned = task.Assignments.Any(a => a.EngineerId == _currentUserService.UserId);

        if (!isAssigned)
            return Result.Failure(TaskErrors.NotAssignedToYou);

        if (task.Status is DomainTaskStatus.Completed or DomainTaskStatus.Closed)
            return Result.Failure(TaskErrors.InvalidStatusForAction);

        // الجزء الجديد: تأكد إن كل التاسكات اللي محتاجة تخلص الأول فعلاً خلصت
        var hasIncompleteDependencies = task.Dependencies.Any(d =>
            d.DependsOnTask.Status != DomainTaskStatus.Completed &&
            d.DependsOnTask.Status != DomainTaskStatus.Closed);

        if (hasIncompleteDependencies)
            return Result.Failure(TaskErrors.DependenciesNotCompleted);

        task.Status = DomainTaskStatus.SubmittedForReview;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}