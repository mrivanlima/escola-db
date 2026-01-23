using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class TeacherCertificationConfiguration : IEntityTypeConfiguration<TeacherCertification>
{
    public void Configure(EntityTypeBuilder<TeacherCertification> builder)
    {
        builder.ToTable("teacher_certifications", "school");

        builder.HasKey(tc => new { tc.TeacherId, tc.CertificationId })
            .HasName("pk_teacher_certifications");

        builder.Property(tc => tc.TeacherId)
            .HasColumnName("teacher_id")
            .IsRequired();

        builder.Property(tc => tc.CertificationId)
            .HasColumnName("certification_id")
            .IsRequired();

        builder.Property(tc => tc.ObtainedDate)
            .HasColumnName("obtained_date");

        builder.Property(tc => tc.ExpiryDate)
            .HasColumnName("expiry_date");

        builder.Property(tc => tc.CredentialNumber)
            .HasColumnName("credential_number");

        builder.Property(tc => tc.Notes)
            .HasColumnName("notes");

        builder.Property(tc => tc.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(tc => tc.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(tc => tc.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(tc => tc.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(tc => tc.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(tc => tc.Teacher)
            .WithMany()
            .HasForeignKey(tc => tc.TeacherId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_teacher_certifications_teacher");

        builder.HasOne(tc => tc.Certification)
            .WithMany()
            .HasForeignKey(tc => tc.CertificationId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teacher_certifications_certification");

        builder.HasOne(tc => tc.CreatedByUser)
            .WithMany()
            .HasForeignKey(tc => tc.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teacher_certifications_created");

        builder.HasOne(tc => tc.UpdatedByUser)
            .WithMany()
            .HasForeignKey(tc => tc.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_teacher_certifications_updated");

        builder.HasIndex(tc => tc.TeacherId)
            .HasDatabaseName("idx_teacher_certifications_teacher");

        builder.HasIndex(tc => tc.CertificationId)
            .HasDatabaseName("idx_teacher_certifications_certification");

        builder.HasIndex(tc => tc.ExpiryDate)
            .HasDatabaseName("idx_teacher_certifications_expiry")
            .HasFilter("expiry_date IS NOT NULL");

        builder.HasIndex(tc => tc.DeletedAt)
            .HasDatabaseName("idx_teacher_certifications_deleted_at");

        builder.HasQueryFilter(tc => tc.DeletedAt == null);
    }
}
