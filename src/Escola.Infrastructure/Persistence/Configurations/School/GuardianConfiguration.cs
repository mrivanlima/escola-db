using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class GuardianConfiguration : IEntityTypeConfiguration<Guardian>
{
    public void Configure(EntityTypeBuilder<Guardian> builder)
    {
        // Table mapping
        builder.ToTable("guardians", "school");

        // Primary key
        builder.HasKey(g => g.GuardianId);
        builder.Property(g => g.GuardianId)
            .HasColumnName("guardian_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(g => g.GuardianUuid)
            .HasColumnName("guardian_uuid")
            .IsRequired();

        builder.Property(g => g.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(g => g.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(g => g.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20);

        builder.Property(g => g.RelationshipTypeId)
            .HasColumnName("relationship_type_id")
            .IsRequired();

        builder.Property(g => g.IsPrimary)
            .HasColumnName("is_primary")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(g => g.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(g => g.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(g => g.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(g => g.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(g => g.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(g => g.Tenant)
            .WithMany()
            .HasForeignKey(g => g.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_guardians_tenant");

        builder.HasOne(g => g.User)
            .WithMany()
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_guardians_user");

        builder.HasOne(g => g.RelationshipType)
            .WithMany()
            .HasForeignKey(g => g.RelationshipTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_guardians_relationship");

        builder.HasOne(g => g.CreatedByUser)
            .WithMany()
            .HasForeignKey(g => g.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_guardians_created")
            .IsRequired(false);

        builder.HasOne(g => g.UpdatedByUser)
            .WithMany()
            .HasForeignKey(g => g.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_guardians_updated")
            .IsRequired(false);

        // Indexes
        builder.HasIndex(g => g.GuardianUuid)
            .IsUnique()
            .HasDatabaseName("uq_guardians_uuid");

        builder.HasIndex(g => g.UserId)
            .IsUnique()
            .HasDatabaseName("uq_guardians_user");

        builder.HasIndex(g => g.TenantId)
            .HasDatabaseName("idx_guardians_tenant");

        builder.HasIndex(g => g.DeletedAt)
            .HasDatabaseName("idx_guardians_deleted_at")
            .HasFilter("deleted_at IS NULL");

        // Query filter for soft delete
        builder.HasQueryFilter(g => g.DeletedAt == null);
    }
}
