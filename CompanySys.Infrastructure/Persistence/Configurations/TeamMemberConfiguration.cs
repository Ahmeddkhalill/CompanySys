namespace CompanySys.Infrastructure.Persistence.Configurations;

public class TeamMemberConfiguration
    : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");

        builder.HasKey(x => new
        {
            x.TeamId,
            x.EngineerId
        });

        builder.HasOne(x => x.Team)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.EngineerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}