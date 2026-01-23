using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class ClassStudentConfiguration : IEntityTypeConfiguration<ClassStudent>
{
    public void Configure(EntityTypeBuilder<ClassStudent> builder)
    {
        // Table mapping
        builder.ToTable("class_students", "school");

        // Primary key
        builder.HasKey(cs => cs.ClassStudentId);
        builder.Property(cs => cs.ClassStudentId)
            .HasColumnName("class_student_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(cs => cs.ClassId)
            .HasColumnName("class_id")
            .IsRequired();

        builder.Property(cs => cs.StudentId)
            .HasColumnName("student_id")
            .IsRequired();

        builder.Property(cs => cs.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(cs => cs.EnrollmentDate)
            .HasColumnName("enrollment_date");

        builder.Property(cs => cs.Status)
            .HasColumnName("status")
            .HasMaxLength(50);

        builder.Property(cs => cs.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(cs => cs.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(cs => cs.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(cs => cs.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(cs => cs.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(cs => cs.Class)
            .WithMany(c => c.ClassStudents)
            .HasForeignKey(cs => cs.ClassId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_class_students_class_id");

        builder.HasOne(cs => cs.Student)
            .WithMany(s => s.ClassStudents)
            .HasForeignKey(cs => cs.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_class_students_student_id");

        builder.HasOne(cs => cs.Tenant)
            .WithMany()
            .HasForeignKey(cs => cs.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_class_students_tenant_id");

        // Indexes
        builder.HasIndex(cs => new { cs.ClassId, cs.StudentId })
            .IsUnique()
            .HasDatabaseName("uq_class_students_class_id_student_id");

        builder.HasIndex(cs => cs.TenantId)
            .HasDatabaseName("idx_class_students_tenant_id");

        builder.HasIndex(cs => cs.DeletedAt)
            .HasDatabaseName("idx_class_students_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(cs => cs.DeletedAt == null);
    }
}
