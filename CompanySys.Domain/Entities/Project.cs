using CompanySys.Domain.Common;
using CompanySys.Domain.Enums;

namespace CompanySys.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ProjectStatus Status { get; set; }

    public string CustomerId { get; set; } = null!;

    public ICollection<ProjectTeam> Teams { get; set; } = new List<ProjectTeam>();

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}