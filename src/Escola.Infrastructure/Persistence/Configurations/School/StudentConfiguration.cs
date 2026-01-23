using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Table mapping
        builder.ToTable("students", "school");

        // Primary key
        builder.HasKey(s => s.StudentId);
        builder.Property(s => s.StudentId)
            .HasColumnName("student_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(s => s.StudentUuid)
            .HasColumnName("student_uuid")
            .IsRequired();

        builder.Property(s => s.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .IsRequired(false);

        builder.Property(s => s.Nickname)
            .HasColumnName("nickname")
            .HasMaxLength(100);

        builder.Property(s => s.NicknameNormalized)
            .HasColumnName("nickname_normalized")
            .HasMaxLength(100)
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(s => s.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.FirstNameNormalized)
            .HasColumnName("first_name_normalized")
            .IsRequired()
            .HasMaxLength(100)
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(s => s.MiddleName)
            .HasColumnName("middle_name")
            .HasMaxLength(100);

        builder.Property(s => s.MiddleNameNormalized)
            .HasColumnName("middle_name_normalized")
            .HasMaxLength(100)
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(s => s.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastNameNormalized)
            .HasColumnName("last_name_normalized")
            .IsRequired()
            .HasMaxLength(100)
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(s => s.BirthDate)
            .HasColumnName("birth_date")
            .IsRequired();

        builder.Property(s => s.AvatarConfig)
            .HasColumnName("avatar_config")
            .HasColumnType("jsonb");

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

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
            .WithMany(t => t.Students)
            .HasForeignKey(s => s.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_students_tenant_id");

        builder.HasOne<Domain.Identity.AppUser>()
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_students_user")
            .IsRequired(false);

        builder.HasOne(s => s.Creator)
            .WithMany()
            .HasForeignKey(s => s.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_students_created_by");

        // Indexes
        builder.HasIndex(s => s.StudentUuid)
            .IsUnique()
            .HasDatabaseName("uq_students_student_uuid");

        builder.HasIndex(s => s.TenantId)
            .HasDatabaseName("idx_students_tenant_id");

        builder.HasIndex(s => s.FirstNameNormalized)
            .HasDatabaseName("idx_students_first_name_normalized");

        builder.HasIndex(s => s.LastNameNormalized)
            .HasDatabaseName("idx_students_last_name_normalized");

        builder.HasIndex(s => s.DeletedAt)
            .HasDatabaseName("idx_students_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(s => s.DeletedAt == null);
    }
}
