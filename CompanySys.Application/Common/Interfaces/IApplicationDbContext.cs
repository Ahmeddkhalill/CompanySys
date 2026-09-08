using CompanySys.Domain.Entities;

namespace CompanySys.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; }
    DbSet<Department> Departments { get; }
    DbSet<Team> Teams { get; }
    DbSet<TeamMember> TeamMembers { get; }
    DbSet<Project> Projects { get; }
    DbSet<ProjectTeam> ProjectTeams { get; }
    DbSet<TaskItem> Tasks { get; }
    DbSet<TaskAssignment> TaskAssignments { get; }
    DbSet<TaskDependency> TaskDependencies { get; }
    DbSet<AuditLog> AuditLogs { get; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
