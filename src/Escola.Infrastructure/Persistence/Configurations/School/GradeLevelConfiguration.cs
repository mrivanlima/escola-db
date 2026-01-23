using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

/// <summary>
/// EF Core configuration for GradeLevel entity.
/// Maps to school.grade_levels table.
/// </summary>
public class GradeLevelConfiguration : IEntityTypeConfiguration<GradeLevel>
{
    public void Configure(EntityTypeBuilder<GradeLevel> builder)
    {
        builder.ToTable("grade_levels", "school");

        // Primary Key
        builder.HasKey(g => g.GradeLevelId)
            .HasName("pk_grade_levels");

        // Properties
        builder.Property(g => g.GradeLevelId)
            .HasColumnName("grade_level_id")
            .ValueGeneratedOnAdd();

        builder.Property(g => g.GradeLevelUuid)
            .HasColumnName("grade_uuid")
            .IsRequired();

        builder.Property(g => g.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(g => g.Name)
            .HasColumnName("grade_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.NameNormalized)
            .HasColumnName("grade_name_normalized")
            .HasMaxLength(100)
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(g => g.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired();

        builder.Property(g => g.Description)
            .HasColumnName("description");

        builder.Property(g => g.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        // Audit fields
        builder.Property(g => g.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(g => g.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(g => g.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(g => g.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(g => g.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(g => g.Tenant)
            .WithMany()
            .HasForeignKey(g => g.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_grade_levels_tenant");

        // Indexes
        builder.HasIndex(g => g.GradeLevelUuid)
            .IsUnique()
            .HasDatabaseName("uq_grade_levels_uuid");

        builder.HasIndex(g => g.TenantId)
            .HasDatabaseName("idx_grade_levels_tenant");

        builder.HasIndex(g => g.NameNormalized)
            .HasDatabaseName("idx_grade_levels_code_normalized");

        builder.HasIndex(g => g.DisplayOrder)
            .HasDatabaseName("idx_grade_levels_display_order");

        builder.HasIndex(g => g.DeletedAt)
            .HasDatabaseName("idx_grade_levels_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(g => g.DeletedAt == null);
    }
}
