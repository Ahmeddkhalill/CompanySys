using CompanySys.Domain.Common;
using CompanySys.Domain.Enums;

namespace CompanySys.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TaskPriority Priority { get; set; }

    public CompanySys.Domain.Enums.TaskStatus Status { get; set; }

    public int Progress { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid TeamId { get; set; }

    public Team Team { get; set; } = null!;

    public Guid ProjectId { get; set; }

    public Project Project { get; set; } = null!;

    public ICollection<TaskAssignment> Assignments { get; set; }
        = new List<TaskAssignment>();

    public ICollection<TaskDependency> Dependencies { get; set; }
        = new List<TaskDependency>();

    public ICollection<TaskDependency> DependentOnTasks { get; set; }
        = new List<TaskDependency>();
}