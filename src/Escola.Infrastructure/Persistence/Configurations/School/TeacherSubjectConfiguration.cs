using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
{
    public void Configure(EntityTypeBuilder<TeacherSubject> builder)
    {
        builder.ToTable("teacher_subjects", "school");

        builder.HasKey(ts => new { ts.TeacherId, ts.SubjectId })
            .HasName("pk_teacher_subjects");

        builder.Property(ts => ts.TeacherId)
            .HasColumnName("teacher_id")
            .IsRequired();

        builder.Property(ts => ts.SubjectId)
            .HasColumnName("subject_id")
            .IsRequired();

        builder.Property(ts => ts.ProficiencyLevelId)
            .HasColumnName("proficiency_level_id");

        builder.Property(ts => ts.YearsExperience)
            .HasColumnName("years_experience");

        builder.Property(ts => ts.IsPrimarySubject)
            .HasColumnName("is_primary_subject")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ts => ts.Notes)
            .HasColumnName("notes");

        builder.Property(ts => ts.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(ts => ts.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(ts => ts.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(ts => ts.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(ts => ts.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(ts => ts.Teacher)
            .WithMany()
            .HasForeignKey(ts => ts.TeacherId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_teacher_subjects_teacher");

        builder.HasOne(ts => ts.Subject)
            .WithMany()
            .HasForeignKey(ts => ts.SubjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teacher_subjects_subject");

        builder.HasOne(ts => ts.ProficiencyLevel)
            .WithMany()
            .HasForeignKey(ts => ts.ProficiencyLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teacher_subjects_proficiency");

        builder.HasOne(ts => ts.CreatedByUser)
            .WithMany()
            .HasForeignKey(ts => ts.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teacher_subjects_created");

        builder.HasOne(ts => ts.UpdatedByUser)
            .WithMany()
            .HasForeignKey(ts => ts.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teacher_subjects_updated");

        builder.HasIndex(ts => ts.TeacherId)
            .HasDatabaseName("idx_teacher_subjects_teacher");

        builder.HasIndex(ts => ts.SubjectId)
            .HasDatabaseName("idx_teacher_subjects_subject");

        builder.HasIndex(ts => new { ts.TeacherId, ts.IsPrimarySubject })
            .HasDatabaseName("idx_teacher_subjects_primary")
            .HasFilter("is_primary_subject = TRUE");

        builder.HasIndex(ts => ts.DeletedAt)
            .HasDatabaseName("idx_teacher_subjects_deleted_at");

        builder.HasQueryFilter(ts => ts.DeletedAt == null);
    }
}
