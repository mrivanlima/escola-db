using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Game;

public class BadgeTypeConfiguration : IEntityTypeConfiguration<Domain.Game.BadgeType>
{
    public void Configure(EntityTypeBuilder<Domain.Game.BadgeType> builder)
    {
        builder.ToTable("badge_types", "game");

        builder.HasKey(bt => bt.BadgeTypeId)
            .HasName("pk_badge_types");

        builder.Property(bt => bt.BadgeTypeId)
            .HasColumnName("badge_type_id")
            .UseIdentityAlwaysColumn();

        builder.Property(bt => bt.BadgeTypeUuid)
            .HasColumnName("badge_type_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(bt => bt.BadgeTypeUuid)
            .IsUnique()
            .HasDatabaseName("uq_badge_types_uuid");

        builder.Property(bt => bt.TypeCode)
            .HasColumnName("type_code")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(bt => bt.TypeCode)
            .IsUnique()
            .HasDatabaseName("uq_badge_types_code");

        builder.Property(bt => bt.TypeCodeNormalized)
            .HasColumnName("type_code_normalized")
            .HasMaxLength(100)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(type_code))", stored: true);

        builder.HasIndex(bt => bt.TypeCodeNormalized)
            .HasDatabaseName("idx_badge_types_code_normalized");

        builder.Property(bt => bt.TypeName)
            .HasColumnName("type_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(bt => bt.TypeNameNormalized)
            .HasColumnName("type_name_normalized")
            .HasMaxLength(200)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(type_name))", stored: true);

        builder.HasIndex(bt => bt.TypeNameNormalized)
            .HasDatabaseName("idx_badge_types_name_normalized");

        builder.Property(bt => bt.Description)
            .HasColumnName("description");

        builder.Property(bt => bt.IconName)
            .HasColumnName("icon_name")
            .HasMaxLength(100);

        builder.Property(bt => bt.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(bt => bt.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(bt => bt.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(bt => bt.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(bt => bt.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(bt => bt.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(bt => bt.DeletedAt)
            .HasDatabaseName("idx_badge_types_deleted_at")
            .HasFilter("deleted_at IS NULL");

        // Navigation
        builder.HasMany(bt => bt.Badges)
            .WithOne(b => b.BadgeType)
            .HasForeignKey(b => b.BadgeTypeId)
            .HasConstraintName("fk_badges_badge_type")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
