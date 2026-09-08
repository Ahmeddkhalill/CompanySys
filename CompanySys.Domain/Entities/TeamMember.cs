namespace CompanySys.Domain.Entities;

public class TeamMember
{
    public Guid TeamId { get; set; }

    public Team Team { get; set; } = null!;

    public string EngineerId { get; set; } = null!;
}