using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("subjects", "school");

        builder.HasKey(s => s.SubjectId)
            .HasName("pk_subjects");

        builder.Property(s => s.SubjectId)
            .HasColumnName("subject_id")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.SubjectUuid)
            .HasColumnName("subject_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SubjectName)
            .HasColumnName("subject_name")
            .IsRequired();

        builder.Property(s => s.SubjectNameNormalized)
            .HasColumnName("subject_name_normalized")
            .IsRequired()
            .HasComputedColumnSql("LOWER(immutable_unaccent(subject_name))", stored: true);

        builder.Property(s => s.Description)
            .HasColumnName("description");

        builder.Property(s => s.GradeLevelId)
            .HasColumnName("grade_level_id");

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(s => s.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(s => s.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(s => s.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(s => s.GradeLevel)
            .WithMany()
            .HasForeignKey(s => s.GradeLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_subjects_grade_level");

        builder.HasOne(s => s.CreatedByUser)
            .WithMany()
            .HasForeignKey(s => s.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_subjects_created");

        builder.HasOne(s => s.UpdatedByUser)
            .WithMany()
            .HasForeignKey(s => s.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_subjects_updated");

        builder.HasIndex(s => s.SubjectUuid)
            .IsUnique()
            .HasDatabaseName("uq_subjects_uuid");

        builder.HasIndex(s => s.SubjectName)
            .IsUnique()
            .HasDatabaseName("uq_subjects_name");

        builder.HasIndex(s => s.SubjectNameNormalized)
            .HasDatabaseName("idx_subjects_name_normalized");

        builder.HasIndex(s => s.GradeLevelId)
            .HasDatabaseName("idx_subjects_grade_level");

        builder.HasIndex(s => s.DeletedAt)
            .HasDatabaseName("idx_subjects_deleted_at");

        builder.HasQueryFilter(s => s.DeletedAt == null);
    }
}
