using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("teachers", "school");

        builder.HasKey(t => t.TeacherId);
        builder.Property(t => t.TeacherId)
            .HasColumnName("teacher_id")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TeacherUuid)
            .HasColumnName("teacher_uuid")
            .IsRequired();

        builder.Property(t => t.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(t => t.SpecializationId)
            .HasColumnName("specialization_id");

        builder.Property(t => t.HireDate)
            .HasColumnName("hire_date");

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

        builder.HasOne(t => t.Tenant)
            .WithMany()
            .HasForeignKey(t => t.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teachers_tenant");

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teachers_user");

        builder.HasOne(t => t.Specialization)
            .WithMany()
            .HasForeignKey(t => t.SpecializationId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teachers_specialization");

        builder.HasOne(t => t.CreatedByUser)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teachers_created");

        builder.HasOne(t => t.UpdatedByUser)
            .WithMany()
            .HasForeignKey(t => t.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teachers_updated");

        builder.HasIndex(t => t.TeacherUuid)
            .IsUnique()
            .HasDatabaseName("uq_teachers_uuid");

        builder.HasIndex(t => t.UserId)
            .IsUnique()
            .HasDatabaseName("uq_teachers_user");

        builder.HasIndex(t => t.TenantId)
            .HasDatabaseName("idx_teachers_tenant");

        builder.HasIndex(t => t.DeletedAt)
            .HasDatabaseName("idx_teachers_deleted_at");

        builder.HasQueryFilter(t => t.DeletedAt == null);
    }
}
