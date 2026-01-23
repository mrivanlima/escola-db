using Escola.Domain.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Game;

public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
{
    public void Configure(EntityTypeBuilder<Badge> builder)
    {
        // Table mapping
        builder.ToTable("badges", "game");

        // Primary key
        builder.HasKey(b => b.BadgeId);
        builder.Property(b => b.BadgeId)
            .HasColumnName("badge_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(b => b.BadgeUuid)
            .HasColumnName("badge_uuid")
            .IsRequired();

        builder.Property(b => b.BadgeName)
            .HasColumnName("badge_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.BadgeNameNormalized)
            .HasColumnName("badge_name_normalized")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(b => b.DescriptionNormalized)
            .HasColumnName("description_normalized")
            .HasMaxLength(1000);

        builder.Property(b => b.BadgeType)
            .HasColumnName("badge_type")
            .HasMaxLength(50);

        builder.Property(b => b.IconUrl)
            .HasColumnName("icon_url")
            .HasMaxLength(1000);

        builder.Property(b => b.Rarity)
            .HasColumnName("rarity")
            .HasMaxLength(50);

        builder.Property(b => b.PointsValue)
            .HasColumnName("points_value");

        builder.Property(b => b.UnlockCriteria)
            .HasColumnName("unlock_criteria")
            .HasColumnType("jsonb");

        builder.Property(b => b.DisplayOrder)
            .HasColumnName("display_order");

        builder.Property(b => b.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(b => b.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(b => b.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(b => b.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(b => b.BadgeUuid)
            .IsUnique()
            .HasDatabaseName("uq_badges_badge_uuid");

        builder.HasIndex(b => b.BadgeNameNormalized)
            .HasDatabaseName("idx_badges_badge_name_normalized");

        builder.HasIndex(b => b.BadgeType)
            .HasDatabaseName("idx_badges_badge_type");

        builder.HasIndex(b => b.DeletedAt)
            .HasDatabaseName("idx_badges_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(b => b.DeletedAt == null);
    }
}
