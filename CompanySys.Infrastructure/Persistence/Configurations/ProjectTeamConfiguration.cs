namespace CompanySys.Infrastructure.Persistence.Configurations;

public class ProjectTeamConfiguration : IEntityTypeConfiguration<ProjectTeam>
{
    public void Configure(EntityTypeBuilder<ProjectTeam> builder)
    {
        builder.ToTable("ProjectTeams");

        builder.HasKey(x => new
        {
            x.ProjectId,
            x.TeamId
        });

        builder.HasOne(x => x.Project)
            .WithMany(x => x.Teams)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Team)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}