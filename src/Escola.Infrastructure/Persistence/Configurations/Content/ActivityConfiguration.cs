using Escola.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Content;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        // Table mapping
        builder.ToTable("activities", "content");

        // Primary key
        builder.HasKey(a => a.ActivityId);
        builder.Property(a => a.ActivityId)
            .HasColumnName("activity_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(a => a.ActivityUuid)
            .HasColumnName("activity_uuid")
            .IsRequired();

        builder.Property(a => a.ModuleId)
            .HasColumnName("module_id")
            .IsRequired();

        builder.Property(a => a.ActivityName)
            .HasColumnName("activity_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.ActivityNameNormalized)
            .HasColumnName("activity_name_normalized")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .HasColumnName("description")
            .HasMaxLength(2000);

        builder.Property(a => a.DescriptionNormalized)
            .HasColumnName("description_normalized")
            .HasMaxLength(2000);

        builder.Property(a => a.ActivityType)
            .HasColumnName("activity_type")
            .HasMaxLength(50);

        builder.Property(a => a.DisplayOrder)
            .HasColumnName("display_order");

        builder.Property(a => a.EstimatedDuration)
            .HasColumnName("estimated_duration");

        builder.Property(a => a.PointsReward)
            .HasColumnName("points_reward");

        builder.Property(a => a.ActivityData)
            .HasColumnName("activity_data")
            .HasColumnType("jsonb");

        builder.Property(a => a.ThumbnailUrl)
            .HasColumnName("thumbnail_url")
            .HasMaxLength(1000);

        builder.Property(a => a.IsPublished)
            .HasColumnName("is_published")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(a => a.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(a => a.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(a => a.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(a => a.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(a => a.Module)
            .WithMany(m => m.Activities)
            .HasForeignKey(a => a.ModuleId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_activities_module_id");

        // Indexes
        builder.HasIndex(a => a.ActivityUuid)
            .IsUnique()
            .HasDatabaseName("uq_activities_activity_uuid");

        builder.HasIndex(a => a.ModuleId)
            .HasDatabaseName("idx_activities_module_id");

        builder.HasIndex(a => a.ActivityNameNormalized)
            .HasDatabaseName("idx_activities_activity_name_normalized");

        builder.HasIndex(a => a.ActivityType)
            .HasDatabaseName("idx_activities_activity_type");

        builder.HasIndex(a => a.DeletedAt)
            .HasDatabaseName("idx_activities_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(a => a.DeletedAt == null);
    }
}
