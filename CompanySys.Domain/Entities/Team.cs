using CompanySys.Domain.Common;

namespace CompanySys.Domain.Entities;

public class Team : BaseEntity
{
    public string Name { get; set; } = null!;

    public Guid DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public string TeamLeadId { get; set; } = null!;

    public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();

    public ICollection<ProjectTeam> Projects { get; set; } = new List<ProjectTeam>();

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}