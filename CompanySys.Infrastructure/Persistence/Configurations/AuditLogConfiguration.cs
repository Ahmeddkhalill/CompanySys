namespace CompanySys.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityId)
            .HasMaxLength(100);

        builder.Property(x => x.Details)
            .HasMaxLength(4000);

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.Timestamp);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}