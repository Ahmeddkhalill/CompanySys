namespace CompanySys.Domain.Entities;

public class TaskDependency
{
    public Guid TaskId { get; set; }

    public TaskItem Task { get; set; } = null!;

    public Guid DependsOnTaskId { get; set; }

    public TaskItem DependsOnTask { get; set; } = null!;
}