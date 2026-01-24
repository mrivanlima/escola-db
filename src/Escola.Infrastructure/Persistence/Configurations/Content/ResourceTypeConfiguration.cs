using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Content;

public class ResourceTypeConfiguration : IEntityTypeConfiguration<Domain.Content.ResourceType>
{
    public void Configure(EntityTypeBuilder<Domain.Content.ResourceType> builder)
    {
        builder.ToTable("resource_types", "content");

        builder.HasKey(rt => rt.ResourceTypeId)
            .HasName("pk_resource_types");

        builder.Property(rt => rt.ResourceTypeId)
            .HasColumnName("resource_type_id")
            .UseIdentityAlwaysColumn();

        builder.Property(rt => rt.ResourceTypeUuid)
            .HasColumnName("resource_type_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(rt => rt.ResourceTypeUuid)
            .IsUnique()
            .HasDatabaseName("uq_resource_types_uuid");

        builder.Property(rt => rt.ResourceTypeCode)
            .HasColumnName("resource_type_code")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(rt => rt.ResourceTypeCode)
            .IsUnique()
            .HasDatabaseName("uq_resource_types_code");

        builder.Property(rt => rt.ResourceTypeCodeNormalized)
            .HasColumnName("resource_type_code_normalized")
            .HasMaxLength(100)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(resource_type_code))", stored: true);

        builder.HasIndex(rt => rt.ResourceTypeCodeNormalized)
            .HasDatabaseName("idx_resource_types_code_normalized");

        builder.Property(rt => rt.ResourceTypeName)
            .HasColumnName("resource_type_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(rt => rt.ResourceTypeNameNormalized)
            .HasColumnName("resource_type_name_normalized")
            .HasMaxLength(200)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(resource_type_name))", stored: true);

        builder.HasIndex(rt => rt.ResourceTypeNameNormalized)
            .HasDatabaseName("idx_resource_types_name_normalized");

        builder.Property(rt => rt.Description)
            .HasColumnName("description");

        builder.Property(rt => rt.MimeTypes)
            .HasColumnName("mime_types")
            .HasColumnType("jsonb");

        builder.Property(rt => rt.MaxFileSizeMb)
            .HasColumnName("max_file_size_mb");

        builder.Property(rt => rt.IconName)
            .HasColumnName("icon_name")
            .HasMaxLength(100);

        builder.Property(rt => rt.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(rt => rt.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(rt => rt.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(rt => rt.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(rt => rt.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(rt => rt.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(rt => rt.DeletedAt)
            .HasDatabaseName("idx_resource_types_deleted_at")
            .HasFilter("deleted_at IS NULL");

        // Navigation
        builder.HasMany(rt => rt.ActivityResources)
            .WithOne(ar => ar.ResourceType)
            .HasForeignKey(ar => ar.ResourceTypeId)
            .HasConstraintName("fk_activity_resources_resource_type")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
