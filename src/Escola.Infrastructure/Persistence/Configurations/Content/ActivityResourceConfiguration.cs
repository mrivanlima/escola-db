using Escola.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Content;

public class ActivityResourceConfiguration : IEntityTypeConfiguration<ActivityResource>
{
    public void Configure(EntityTypeBuilder<ActivityResource> builder)
    {
        // Table mapping
        builder.ToTable("activity_resources", "content");

        // Primary key
        builder.HasKey(ar => ar.ResourceId);
        builder.Property(ar => ar.ResourceId)
            .HasColumnName("resource_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(ar => ar.ResourceUuid)
            .HasColumnName("resource_uuid")
            .IsRequired();

        builder.Property(ar => ar.ActivityId)
            .HasColumnName("activity_id")
            .IsRequired();

        builder.Property(ar => ar.ResourceName)
            .HasColumnName("resource_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ar => ar.ResourceNameNormalized)
            .HasColumnName("resource_name_normalized")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ar => ar.ResourceTypeId)
            .HasColumnName("resource_type_id");

        builder.Property(ar => ar.MediaFileId)
            .HasColumnName("media_file_id")
            .IsRequired();

        builder.Property(ar => ar.DisplayOrder)
            .HasColumnName("display_order");

        builder.Property(ar => ar.IsRequired)
            .HasColumnName("is_required")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ar => ar.UsageContextId)
            .HasColumnName("usage_context_id");

        builder.Property(ar => ar.ResourceConfig)
            .HasColumnName("resource_config")
            .HasColumnType("jsonb");

        builder.Property(ar => ar.IsPublished)
            .HasColumnName("is_published")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ar => ar.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(ar => ar.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(ar => ar.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(ar => ar.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(ar => ar.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(ar => ar.Activity)
            .WithMany(a => a.ActivityResources)
            .HasForeignKey(ar => ar.ActivityId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_activity_resources_activity_id");

        builder.HasOne(ar => ar.ResourceType)
            .WithMany(rt => rt.ActivityResources)
            .HasForeignKey(ar => ar.ResourceTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_activity_resources_resource_type");

        builder.HasOne(ar => ar.UsageContext)
            .WithMany(uc => uc.ActivityResources)
            .HasForeignKey(ar => ar.UsageContextId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_activity_resources_usage_context");

        builder.HasOne(ar => ar.MediaFile)
            .WithMany(m => m.ActivityResources)
            .HasForeignKey(ar => ar.MediaFileId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_activity_resources_media_file_id");

        // Indexes
        builder.HasIndex(ar => ar.ResourceUuid)
            .IsUnique()
            .HasDatabaseName("uq_activity_resources_resource_uuid");

        builder.HasIndex(ar => ar.ActivityId)
            .HasDatabaseName("idx_activity_resources_activity_id");

        builder.HasIndex(ar => ar.MediaFileId)
            .HasDatabaseName("idx_activity_resources_media_file_id");

        builder.HasIndex(ar => ar.ResourceTypeId)
            .HasDatabaseName("idx_activity_resources_resource_type_id");

        builder.HasIndex(ar => ar.UsageContextId)
            .HasDatabaseName("idx_activity_resources_usage_context_id");

        builder.HasIndex(ar => ar.DeletedAt)
            .HasDatabaseName("idx_activity_resources_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(ar => ar.DeletedAt == null);
    }
}
