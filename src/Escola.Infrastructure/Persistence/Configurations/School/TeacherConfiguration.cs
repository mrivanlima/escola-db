using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        // Table mapping
        builder.ToTable("teachers", "school");

        // Primary key
        builder.HasKey(t => t.TeacherId);
        builder.Property(t => t.TeacherId)
            .HasColumnName("teacher_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(t => t.TeacherUuid)
            .HasColumnName("teacher_uuid")
            .IsRequired();

        builder.Property(t => t.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(t => t.Specialization)
            .HasColumnName("specialization")
            .HasMaxLength(200);

        builder.Property(t => t.SpecializationNormalized)
            .HasColumnName("specialization_normalized")
            .HasMaxLength(200);

        builder.Property(t => t.HireDate)
            .HasColumnName("hire_date");

        builder.Property(t => t.TeacherConfig)
            .HasColumnName("teacher_config")
            .HasColumnType("jsonb");

        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(t => t.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(t => t.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(t => t.Tenant)
            .WithMany(tenant => tenant.Teachers)
            .HasForeignKey(t => t.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teachers_tenant_id");

        builder.HasOne(t => t.User)
            .WithMany(u => u.Teachers)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teachers_user_id");

        // Indexes
        builder.HasIndex(t => t.TeacherUuid)
            .IsUnique()
            .HasDatabaseName("uq_teachers_teacher_uuid");

        builder.HasIndex(t => t.TenantId)
            .HasDatabaseName("idx_teachers_tenant_id");

        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("idx_teachers_user_id");

        builder.HasIndex(t => t.DeletedAt)
            .HasDatabaseName("idx_teachers_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(t => t.DeletedAt == null);
    }
}
