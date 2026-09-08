namespace CompanySys.Domain.Entities;

public class ProjectTeam
{
    public Guid ProjectId { get; set; }

    public Project Project { get; set; } = null!;

    public Guid TeamId { get; set; }

    public Team Team { get; set; } = null!;
}