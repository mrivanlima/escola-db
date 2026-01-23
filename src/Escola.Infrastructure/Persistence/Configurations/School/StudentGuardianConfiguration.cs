using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class StudentGuardianConfiguration : IEntityTypeConfiguration<StudentGuardian>
{
    public void Configure(EntityTypeBuilder<StudentGuardian> builder)
    {
        // Table mapping
        builder.ToTable("student_guardians", "school");

        // Primary key
        builder.HasKey(sg => sg.StudentGuardianId);
        builder.Property(sg => sg.StudentGuardianId)
            .HasColumnName("student_guardian_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(sg => sg.StudentId)
            .HasColumnName("student_id")
            .IsRequired();

        builder.Property(sg => sg.GuardianId)
            .HasColumnName("guardian_id")
            .IsRequired();

        builder.Property(sg => sg.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(sg => sg.RelationshipNotes)
            .HasColumnName("relationship_notes")
            .HasMaxLength(500);

        builder.Property(sg => sg.RelationshipNotesNormalized)
            .HasColumnName("relationship_notes_normalized")
            .HasMaxLength(500);

        builder.Property(sg => sg.IsAuthorizedPickup)
            .HasColumnName("is_authorized_pickup")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(sg => sg.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(sg => sg.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(sg => sg.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(sg => sg.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(sg => sg.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(sg => sg.Student)
            .WithMany(s => s.StudentGuardians)
            .HasForeignKey(sg => sg.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_guardians_student_id");

        builder.HasOne(sg => sg.Guardian)
            .WithMany(g => g.StudentGuardians)
            .HasForeignKey(sg => sg.GuardianId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_guardians_guardian_id");

        builder.HasOne(sg => sg.Tenant)
            .WithMany()
            .HasForeignKey(sg => sg.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_guardians_tenant_id");

        // Indexes
        builder.HasIndex(sg => new { sg.StudentId, sg.GuardianId })
            .IsUnique()
            .HasDatabaseName("uq_student_guardians_student_id_guardian_id");

        builder.HasIndex(sg => sg.TenantId)
            .HasDatabaseName("idx_student_guardians_tenant_id");

        builder.HasIndex(sg => sg.DeletedAt)
            .HasDatabaseName("idx_student_guardians_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(sg => sg.DeletedAt == null);
    }
}
