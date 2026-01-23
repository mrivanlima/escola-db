using Escola.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Identity;

public class TenantTypeConfiguration : IEntityTypeConfiguration<TenantType>
{
    public void Configure(EntityTypeBuilder<TenantType> builder)
    {
        // Table mapping
        builder.ToTable("tenant_types", "identity");

        // Primary key
        builder.HasKey(tt => tt.TenantTypeId);
        builder.Property(tt => tt.TenantTypeId)
            .HasColumnName("tenant_type_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(tt => tt.TenantTypeUuid)
            .HasColumnName("tenant_type_uuid")
            .IsRequired();

        builder.Property(tt => tt.TypeName)
            .HasColumnName("type_name")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(tt => tt.TypeNameNormalized)
            .HasColumnName("type_name_normalized")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(tt => tt.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

        builder.Property(tt => tt.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(tt => tt.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(tt => tt.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(tt => tt.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(tt => tt.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(tt => tt.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(tt => tt.TenantTypeUuid)
            .IsUnique()
            .HasDatabaseName("uq_tenant_types_tenant_type_uuid");

        builder.HasIndex(tt => tt.TypeNameNormalized)
            .HasDatabaseName("idx_tenant_types_type_name_normalized");

        builder.HasIndex(tt => tt.DeletedAt)
            .HasDatabaseName("idx_tenant_types_deleted_at");

        // Relationships
        builder.HasOne(tt => tt.Creator)
            .WithMany()
            .HasForeignKey(tt => tt.CreatedBy)
            .HasConstraintName("fk_tenant_types_created_by")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tt => tt.Updater)
            .WithMany()
            .HasForeignKey(tt => tt.UpdatedBy)
            .HasConstraintName("fk_tenant_types_updated_by")
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(tt => tt.DeletedAt == null);
    }
}
