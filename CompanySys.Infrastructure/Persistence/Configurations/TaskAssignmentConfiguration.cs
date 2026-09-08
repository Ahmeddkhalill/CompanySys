namespace CompanySys.Infrastructure.Persistence.Configurations;

public class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
{
    public void Configure(EntityTypeBuilder<TaskAssignment> builder)
    {
        builder.ToTable("TaskAssignments");

        builder.HasKey(x => new
        {
            x.TaskId,
            x.EngineerId
        });

        builder.HasOne(x => x.Task)
            .WithMany(x => x.Assignments)
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.EngineerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}