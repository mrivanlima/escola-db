using Escola.Domain.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Game;

public class StudentBadgeConfiguration : IEntityTypeConfiguration<StudentBadge>
{
    public void Configure(EntityTypeBuilder<StudentBadge> builder)
    {
        // Table mapping
        builder.ToTable("student_badges", "game");

        // Primary key
        builder.HasKey(sb => sb.StudentBadgeId);
        builder.Property(sb => sb.StudentBadgeId)
            .HasColumnName("student_badge_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(sb => sb.StudentId)
            .HasColumnName("student_id")
            .IsRequired();

        builder.Property(sb => sb.BadgeId)
            .HasColumnName("badge_id")
            .IsRequired();

        builder.Property(sb => sb.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(sb => sb.EarnedAt)
            .HasColumnName("earned_at")
            .IsRequired();

        builder.Property(sb => sb.EarnMetadata)
            .HasColumnName("earn_metadata")
            .HasColumnType("jsonb");

        builder.Property(sb => sb.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(sb => sb.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(sb => sb.Student)
            .WithMany(s => s.StudentBadges)
            .HasForeignKey(sb => sb.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_badges_student_id");

        builder.HasOne(sb => sb.Badge)
            .WithMany(b => b.StudentBadges)
            .HasForeignKey(sb => sb.BadgeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_badges_badge_id");

        builder.HasOne(sb => sb.Tenant)
            .WithMany()
            .HasForeignKey(sb => sb.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_student_badges_tenant_id");

        // Indexes
        builder.HasIndex(sb => new { sb.StudentId, sb.BadgeId })
            .IsUnique()
            .HasDatabaseName("uq_student_badges_student_id_badge_id");

        builder.HasIndex(sb => sb.TenantId)
            .HasDatabaseName("idx_student_badges_tenant_id");

        builder.HasIndex(sb => sb.DeletedAt)
            .HasDatabaseName("idx_student_badges_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(sb => sb.DeletedAt == null);
    }
}
