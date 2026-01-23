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

        builder.Property(g => g.Relationship)
            .HasColumnName("relationship")
            .HasMaxLength(50);

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
            .WithMany(t => t.Guardians)
            .HasForeignKey(g => g.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_guardians_tenant_id");

        builder.HasOne(g => g.User)
            .WithMany(u => u.Guardians)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_guardians_user_id");

        // Indexes
        builder.HasIndex(g => g.GuardianUuid)
            .IsUnique()
            .HasDatabaseName("uq_guardians_guardian_uuid");

        builder.HasIndex(g => g.TenantId)
            .HasDatabaseName("idx_guardians_tenant_id");

        builder.HasIndex(g => g.UserId)
            .HasDatabaseName("idx_guardians_user_id");

        builder.HasIndex(g => g.DeletedAt)
            .HasDatabaseName("idx_guardians_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(g => g.DeletedAt == null);
    }
}
