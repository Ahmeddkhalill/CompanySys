using CompanySys.Domain.Common;

namespace CompanySys.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid CompanyId { get; set; }

    public Company Company { get; set; } = null!;

    public ICollection<Team> Teams { get; set; } = new List<Team>();
}