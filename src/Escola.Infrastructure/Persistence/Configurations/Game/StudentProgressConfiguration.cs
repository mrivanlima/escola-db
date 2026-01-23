using Escola.Domain.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Game;

public class StudentProgressConfiguration : IEntityTypeConfiguration<StudentProgress>
{
    public void Configure(EntityTypeBuilder<StudentProgress> builder)
    {
        // Table mapping
        builder.ToTable("student_progress", "game");

        // Primary key
        builder.HasKey(sp => sp.ProgressId);
        builder.Property(sp => sp.ProgressId)
            .HasColumnName("progress_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(sp => sp.ProgressUuid)
            .HasColumnName("progress_uuid")
            .IsRequired();

        builder.Property(sp => sp.StudentId)
            .HasColumnName("student_id")
            .IsRequired();

        builder.Property(sp => sp.ActivityId)
            .HasColumnName("activity_id")
            .IsRequired();

        builder.Property(sp => sp.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(sp => sp.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(sp => sp.Score)
            .HasColumnName("score");

        builder.Property(sp => sp.Attempts)
            .HasColumnName("attempts")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(sp => sp.TimeSpentSeconds)
            .HasColumnName("time_spent_seconds");

        builder.Property(sp => sp.ProgressData)
            .HasColumnName("progress_data")
            .HasColumnType("jsonb");

        builder.Property(sp => sp.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(sp => sp.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(sp => sp.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(sp => sp.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(sp => sp.Student)
            .WithMany(s => s.StudentProgress)
            .HasForeignKey(sp => sp.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_progress_student_id");

        builder.HasOne(sp => sp.Activity)
            .WithMany(a => a.StudentProgress)
            .HasForeignKey(sp => sp.ActivityId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_progress_activity_id");

        builder.HasOne(sp => sp.Tenant)
            .WithMany()
            .HasForeignKey(sp => sp.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_progress_tenant_id");

        // Indexes
        builder.HasIndex(sp => sp.ProgressUuid)
            .IsUnique()
            .HasDatabaseName("uq_student_progress_progress_uuid");

        builder.HasIndex(sp => new { sp.StudentId, sp.ActivityId })
            .HasDatabaseName("idx_student_progress_student_id_activity_id");

        builder.HasIndex(sp => sp.TenantId)
            .HasDatabaseName("idx_student_progress_tenant_id");

        builder.HasIndex(sp => sp.Status)
            .HasDatabaseName("idx_student_progress_status");

        builder.HasIndex(sp => sp.DeletedAt)
            .HasDatabaseName("idx_student_progress_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(sp => sp.DeletedAt == null);
    }
}
