using CompanySys.Domain.Entities;

namespace CompanySys.Application.Features.Tasks.Commands.AddTaskDependency;

public class AddTaskDependencyCommandHandler : IRequestHandler<AddTaskDependencyCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public AddTaskDependencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(AddTaskDependencyCommand request, CancellationToken cancellationToken)
    {
        // 1. تاسك متعتمدش على نفسها
        if (request.TaskId == request.DependsOnTaskId)
            return Result.Failure(TaskErrors.CannotDependOnSelf);

        // 2. تأكد إن التاسك نفسها موجودة
        var taskExists = await _context.Tasks.AnyAsync(t => t.Id == request.TaskId, cancellationToken);

        if (!taskExists)
            return Result.Failure(TaskErrors.NotFound);

        // 3. تأكد إن التاسك التانية (المعتمد عليها) موجودة
        var dependsOnExists = await _context.Tasks.AnyAsync(t => t.Id == request.DependsOnTaskId, cancellationToken);

        if (!dependsOnExists)
            return Result.Failure(TaskErrors.DependencyTaskNotFound);

        // 4. تأكد إن الاعتمادية دي مش مضافة بالفعل
        var alreadyExists = await _context.TaskDependencies
            .AnyAsync(d => d.TaskId == request.TaskId && d.DependsOnTaskId == request.DependsOnTaskId, cancellationToken);

        if (alreadyExists)
            return Result.Failure(TaskErrors.DependencyAlreadyExists);

        // 5. تأكد إن ده مش هيعمل دورة (Circular Dependency)
        var wouldCreateCycle = await WouldCreateCycleAsync(request.DependsOnTaskId, request.TaskId, cancellationToken);

        if (wouldCreateCycle)
            return Result.Failure(TaskErrors.CircularDependency);

        _context.TaskDependencies.Add(new TaskDependency
        {
            TaskId = request.TaskId,
            DependsOnTaskId = request.DependsOnTaskId
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    // بيدور بادئًا من startTaskId عبر سلسلة الاعتماديات الموجودة،
    // ولو وصل لـ targetTaskId، معناه إضافة الاعتمادية الجديدة هتعمل دورة (loop)
    private async Task<bool> WouldCreateCycleAsync(Guid startTaskId, Guid targetTaskId, CancellationToken cancellationToken)
    {
        var visited = new HashSet<Guid>();
        var queue = new Queue<Guid>();
        queue.Enqueue(startTaskId);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == targetTaskId)
                return true;

            if (!visited.Add(current))
                continue;

            var nextIds = await _context.TaskDependencies
                .Where(d => d.TaskId == current)
                .Select(d => d.DependsOnTaskId)
                .ToListAsync(cancellationToken);

            foreach (var id in nextIds)
                queue.Enqueue(id);
        }

        return false;
    }
}