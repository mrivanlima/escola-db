using Escola.Domain.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Assets;

public class MediaCategoryConfiguration : IEntityTypeConfiguration<MediaCategory>
{
    public void Configure(EntityTypeBuilder<MediaCategory> builder)
    {
        builder.ToTable("media_categories", "assets");

        builder.HasKey(mc => mc.CategoryId)
            .HasName("pk_media_categories");

        builder.Property(mc => mc.CategoryId)
            .HasColumnName("category_id")
            .ValueGeneratedOnAdd();

        builder.Property(mc => mc.CategoryUuid)
            .HasColumnName("category_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(mc => mc.CategoryCode)
            .HasColumnName("category_code")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(mc => mc.CategoryName)
            .HasColumnName("category_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mc => mc.CategoryCodeNormalized)
            .HasColumnName("category_code_normalized")
            .IsRequired()
            .HasComputedColumnSql("LOWER(immutable_unaccent(category_code))", stored: true);

        builder.Property(mc => mc.CategoryNameNormalized)
            .HasColumnName("category_name_normalized")
            .IsRequired()
            .HasComputedColumnSql("LOWER(immutable_unaccent(category_name))", stored: true);

        builder.Property(mc => mc.DefaultIconName)
            .HasColumnName("default_icon_name")
            .HasMaxLength(100);

        builder.Property(mc => mc.DefaultMaxSizeMb)
            .HasColumnName("default_max_size_mb");

        builder.Property(mc => mc.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired();

        builder.Property(mc => mc.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(mc => mc.CategoryUuid)
            .HasDatabaseName("uq_media_categories_uuid")
            .IsUnique();

        builder.HasIndex(mc => mc.CategoryCode)
            .HasDatabaseName("uq_media_categories_code")
            .IsUnique();

        builder.HasIndex(mc => mc.CategoryCodeNormalized)
            .HasDatabaseName("idx_media_categories_code_normalized");

        builder.HasIndex(mc => mc.CategoryNameNormalized)
            .HasDatabaseName("idx_media_categories_name_normalized");

        builder.HasIndex(mc => mc.DisplayOrder)
            .HasDatabaseName("idx_media_categories_display_order");
    }
}
