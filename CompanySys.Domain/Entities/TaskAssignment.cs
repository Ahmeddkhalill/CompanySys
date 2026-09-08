namespace CompanySys.Domain.Entities;

public class TaskAssignment
{
    public Guid TaskId { get; set; }

    public TaskItem Task { get; set; } = null!;

    public string EngineerId { get; set; } = null!;
}