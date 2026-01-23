using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class EnrollmentStatusConfiguration : IEntityTypeConfiguration<EnrollmentStatus>
{
    public void Configure(EntityTypeBuilder<EnrollmentStatus> builder)
    {
        // Table mapping
        builder.ToTable("enrollment_statuses", "school");

        // Primary key
        builder.HasKey(e => e.EnrollmentStatusId)
            .HasName("enrollment_statuses_pkey");

        builder.Property(e => e.EnrollmentStatusId)
            .HasColumnName("enrollment_status_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(e => e.EnrollmentStatusUuid)
            .HasColumnName("enrollment_status_uuid")
            .IsRequired();

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.NameNormalized)
            .HasColumnName("name_normalized")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.DisplayOrder)
            .HasColumnName("display_order")
            .HasDefaultValue(0);

        builder.Property(e => e.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // Relationships
        builder.HasOne(e => e.Tenant)
            .WithMany()
            .HasForeignKey(e => e.TenantId)
            .HasConstraintName("enrollment_statuses_tenant_id_fkey")
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.EnrollmentStatusUuid)
            .HasDatabaseName("enrollment_statuses_enrollment_status_uuid_key")
            .IsUnique();

        builder.HasIndex(e => e.TenantId)
            .HasDatabaseName("idx_enrollment_statuses_tenant");

        builder.HasIndex(e => new { e.TenantId, e.NameNormalized })
            .HasDatabaseName("enrollment_statuses_tenant_id_name_normalized_key")
            .IsUnique();

        builder.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("idx_enrollment_statuses_display_order");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("idx_enrollment_statuses_is_active")
            .HasFilter("is_active = true");
    }
}
