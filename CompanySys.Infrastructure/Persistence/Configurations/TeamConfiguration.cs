namespace CompanySys.Infrastructure.Persistence.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => new
        {
            x.DepartmentId,
            x.Name
        })
        .IsUnique();

        builder.HasOne(x => x.Department)
            .WithMany(x => x.Teams)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.TeamLeadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}