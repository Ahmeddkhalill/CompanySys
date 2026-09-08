namespace CompanySys.Infrastructure.Persistence.Configurations;

public class TaskDependencyConfiguration : IEntityTypeConfiguration<TaskDependency>
{
    public void Configure(EntityTypeBuilder<TaskDependency> builder)
    {
        builder.ToTable("TaskDependencies");

        builder.HasKey(x => new
        {
            x.TaskId,
            x.DependsOnTaskId
        });

        builder.HasOne(x => x.Task)
            .WithMany(x => x.Dependencies)
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DependsOnTask)
            .WithMany(x => x.DependentOnTasks)
            .HasForeignKey(x => x.DependsOnTaskId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}