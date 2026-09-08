using DomainTaskStatus = CompanySys.Domain.Enums.TaskStatus;

namespace CompanySys.Application.Features.Tasks.Commands.ReviewTask;

public class ReviewTaskCommandHandler : IRequestHandler<ReviewTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public ReviewTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ReviewTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            return Result.Failure(TaskErrors.NotFound);

        if (task.Status != DomainTaskStatus.SubmittedForReview)
            return Result.Failure(TaskErrors.InvalidStatusForAction);

        task.Status = request.Approved ? DomainTaskStatus.Completed : DomainTaskStatus.InProgress;
        task.Progress = request.Approved ? 100 : task.Progress;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}