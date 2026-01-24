using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Game;

public class BadgeRarityConfiguration : IEntityTypeConfiguration<Domain.Game.BadgeRarity>
{
    public void Configure(EntityTypeBuilder<Domain.Game.BadgeRarity> builder)
    {
        builder.ToTable("badge_rarities", "game");

        builder.HasKey(br => br.RarityId)
            .HasName("pk_badge_rarities");

        builder.Property(br => br.RarityId)
            .HasColumnName("rarity_id")
            .UseIdentityAlwaysColumn();

        builder.Property(br => br.RarityUuid)
            .HasColumnName("rarity_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(br => br.RarityUuid)
            .IsUnique()
            .HasDatabaseName("uq_badge_rarities_uuid");

        builder.Property(br => br.RarityCode)
            .HasColumnName("rarity_code")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(br => br.RarityCode)
            .IsUnique()
            .HasDatabaseName("uq_badge_rarities_code");

        builder.Property(br => br.RarityCodeNormalized)
            .HasColumnName("rarity_code_normalized")
            .HasMaxLength(100)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(rarity_code))", stored: true);

        builder.HasIndex(br => br.RarityCodeNormalized)
            .HasDatabaseName("idx_badge_rarities_code_normalized");

        builder.Property(br => br.RarityName)
            .HasColumnName("rarity_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(br => br.RarityNameNormalized)
            .HasColumnName("rarity_name_normalized")
            .HasMaxLength(200)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(rarity_name))", stored: true);

        builder.HasIndex(br => br.RarityNameNormalized)
            .HasDatabaseName("idx_badge_rarities_name_normalized");

        builder.Property(br => br.Description)
            .HasColumnName("description");

        builder.Property(br => br.ColorCode)
            .HasColumnName("color_code")
            .HasMaxLength(20);

        builder.Property(br => br.PointMultiplier)
            .HasColumnName("point_multiplier")
            .HasColumnType("NUMERIC(5,2)");

        builder.Property(br => br.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(br => br.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(br => br.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(br => br.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(br => br.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(br => br.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(br => br.DeletedAt)
            .HasDatabaseName("idx_badge_rarities_deleted_at")
            .HasFilter("deleted_at IS NULL");

        // Navigation
        builder.HasMany(br => br.Badges)
            .WithOne(b => b.BadgeRarity)
            .HasForeignKey(b => b.RarityId)
            .HasConstraintName("fk_badges_rarity")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
