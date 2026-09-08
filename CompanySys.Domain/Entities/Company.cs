using CompanySys.Domain.Common;

namespace CompanySys.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<Department> Departments { get; set; } = new List<Department>();
}