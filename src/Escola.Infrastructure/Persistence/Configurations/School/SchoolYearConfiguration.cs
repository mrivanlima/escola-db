using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

/// <summary>
/// EF Core configuration for SchoolYear entity.
/// Maps to school.school_years table.
/// </summary>
public class SchoolYearConfiguration : IEntityTypeConfiguration<SchoolYear>
{
    public void Configure(EntityTypeBuilder<SchoolYear> builder)
    {
        builder.ToTable("school_years", "school");

        // Primary Key
        builder.HasKey(s => s.SchoolYearId)
            .HasName("pk_school_years");

        // Properties
        builder.Property(s => s.SchoolYearId)
            .HasColumnName("school_year_id")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.SchoolYearUuid)
            .HasColumnName("year_uuid")
            .IsRequired();

        builder.Property(s => s.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(s => s.Name)
            .HasColumnName("year_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.NameNormalized)
            .HasColumnName("year_name_normalized")
            .HasMaxLength(100)
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(s => s.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(s => s.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(s => s.IsCurrent)
            .HasColumnName("is_current")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        // Audit fields
        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(s => s.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(s => s.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(s => s.Tenant)
            .WithMany()
            .HasForeignKey(s => s.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_school_years_tenant");

        // Indexes
        builder.HasIndex(s => s.SchoolYearUuid)
            .IsUnique()
            .HasDatabaseName("uq_school_years_uuid");

        builder.HasIndex(s => s.TenantId)
            .HasDatabaseName("idx_school_years_tenant");

        builder.HasIndex(s => s.NameNormalized)
            .HasDatabaseName("idx_school_years_code_normalized");

        builder.HasIndex(s => new { s.TenantId, s.IsCurrent })
            .HasDatabaseName("idx_school_years_is_current")
            .HasFilter("is_current = TRUE");

        builder.HasIndex(s => new { s.StartDate, s.EndDate })
            .HasDatabaseName("idx_school_years_dates");

        builder.HasIndex(s => s.DeletedAt)
            .HasDatabaseName("idx_school_years_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(s => s.DeletedAt == null);
    }
}
