using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class RelationshipTypeConfiguration : IEntityTypeConfiguration<RelationshipType>
{
    public void Configure(EntityTypeBuilder<RelationshipType> builder)
    {
        // Table mapping
        builder.ToTable("relationship_types", "school");

        // Primary key
        builder.HasKey(rt => rt.RelationshipTypeId)
            .HasName("relationship_types_pkey");

        builder.Property(rt => rt.RelationshipTypeId)
            .HasColumnName("relationship_type_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(rt => rt.RelationshipTypeUuid)
            .HasColumnName("relationship_type_uuid")
            .IsRequired();

        builder.Property(rt => rt.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(rt => rt.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(rt => rt.NameNormalized)
            .HasColumnName("name_normalized")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(rt => rt.DisplayOrder)
            .HasColumnName("display_order")
            .HasDefaultValue(0);

        builder.Property(rt => rt.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // Relationships
        builder.HasOne(rt => rt.Tenant)
            .WithMany()
            .HasForeignKey(rt => rt.TenantId)
            .HasConstraintName("relationship_types_tenant_id_fkey")
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(rt => rt.RelationshipTypeUuid)
            .HasDatabaseName("relationship_types_relationship_type_uuid_key")
            .IsUnique();

        builder.HasIndex(rt => rt.TenantId)
            .HasDatabaseName("idx_relationship_types_tenant");

        builder.HasIndex(rt => new { rt.TenantId, rt.NameNormalized })
            .HasDatabaseName("relationship_types_tenant_id_name_normalized_key")
            .IsUnique();

        builder.HasIndex(rt => rt.DisplayOrder)
            .HasDatabaseName("idx_relationship_types_display_order");

        builder.HasIndex(rt => rt.IsActive)
            .HasDatabaseName("idx_relationship_types_is_active")
            .HasFilter("is_active = true");
    }
}
