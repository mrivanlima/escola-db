using Escola.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Identity;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        // Table mapping
        builder.ToTable("tenants", "identity");

        // Primary key
        builder.HasKey(t => t.TenantId);
        builder.Property(t => t.TenantId)
            .HasColumnName("tenant_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(t => t.TenantUuid)
            .HasColumnName("tenant_uuid")
            .IsRequired();

        builder.Property(t => t.TenantName)
            .HasColumnName("tenant_name")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.TenantNameNormalized)
            .HasColumnName("tenant_name_normalized")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.TenantTypeId)
            .HasColumnName("tenant_type_id");

        builder.Property(t => t.TenantConfig)
            .HasColumnName("tenant_config")
            .HasColumnType("jsonb");

        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(t => t.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(t => t.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(t => t.TenantUuid)
            .IsUnique()
            .HasDatabaseName("uq_tenants_tenant_uuid");

        builder.HasIndex(t => t.TenantNameNormalized)
            .HasDatabaseName("idx_tenants_tenant_name_normalized");

        builder.HasIndex(t => t.DeletedAt)
            .HasDatabaseName("idx_tenants_deleted_at");

        // Relationships
        builder.HasOne(t => t.TenantType)
            .WithMany()
            .HasForeignKey(t => t.TenantTypeId)
            .HasConstraintName("fk_tenants_tenant_type_id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Creator)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .HasConstraintName("fk_tenants_created_by")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Updater)
            .WithMany()
            .HasForeignKey(t => t.UpdatedBy)
            .HasConstraintName("fk_tenants_updated_by")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.AppUsers)
            .WithOne(u => u.Tenant)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Students)
            .WithOne(s => s.Tenant)
            .HasForeignKey(s => s.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(t => t.DeletedAt == null);
    }
}
