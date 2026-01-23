using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.ToTable("classes", "school");

        builder.HasKey(c => c.ClassId);
        builder.Property(c => c.ClassId)
            .HasColumnName("class_id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ClassUuid)
            .HasColumnName("class_uuid")
            .IsRequired();

        builder.Property(c => c.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(c => c.ClassName)
            .HasColumnName("class_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.ClassNameNormalized)
            .HasColumnName("class_name_normalized")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.GradeLevelId)
            .HasColumnName("grade_level_id");

        builder.Property(c => c.SchoolYearId)
            .HasColumnName("school_year_id")
            .IsRequired();

        builder.Property(c => c.MaxStudents)
            .HasColumnName("max_students");

        builder.Property(c => c.ClassConfig)
            .HasColumnName("class_config")
            .HasColumnType("jsonb");

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(c => c.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(c => c.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(c => c.Tenant)
            .WithMany()
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_classes_tenant_id");

        builder.HasOne(c => c.GradeLevel)
            .WithMany()
            .HasForeignKey(c => c.GradeLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_classes_grade_level_id");

        builder.HasOne(c => c.SchoolYear)
            .WithMany()
            .HasForeignKey(c => c.SchoolYearId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_classes_school_year_id");

        builder.HasOne(c => c.CreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_classes_created_by");

        builder.HasOne(c => c.UpdatedByUser)
            .WithMany()
            .HasForeignKey(c => c.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_classes_updated_by");

        builder.HasIndex(c => c.ClassUuid)
            .IsUnique()
            .HasDatabaseName("uq_classes_class_uuid");

        builder.HasIndex(c => c.TenantId)
            .HasDatabaseName("idx_classes_tenant_id");

        builder.HasIndex(c => c.ClassNameNormalized)
            .HasDatabaseName("idx_classes_class_name_normalized");

        builder.HasIndex(c => c.DeletedAt)
            .HasDatabaseName("idx_classes_deleted_at");

        builder.HasQueryFilter(c => c.DeletedAt == null);
    }
}
