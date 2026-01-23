using Escola.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Content;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        // Table mapping
        builder.ToTable("modules", "content");

        // Primary key
        builder.HasKey(m => m.ModuleId);
        builder.Property(m => m.ModuleId)
            .HasColumnName("module_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(m => m.ModuleUuid)
            .HasColumnName("module_uuid")
            .IsRequired();

        builder.Property(m => m.ModuleName)
            .HasColumnName("module_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.ModuleNameNormalized)
            .HasColumnName("module_name_normalized")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Description)
            .HasColumnName("description")
            .HasMaxLength(2000);

        builder.Property(m => m.DescriptionNormalized)
            .HasColumnName("description_normalized")
            .HasMaxLength(2000);

        builder.Property(m => m.ModuleType)
            .HasColumnName("module_type")
            .HasMaxLength(50);

        builder.Property(m => m.DifficultyLevel)
            .HasColumnName("difficulty_level");

        builder.Property(m => m.RecommendedAgeMin)
            .HasColumnName("recommended_age_min");

        builder.Property(m => m.RecommendedAgeMax)
            .HasColumnName("recommended_age_max");

        builder.Property(m => m.DisplayOrder)
            .HasColumnName("display_order");

        builder.Property(m => m.ThumbnailUrl)
            .HasColumnName("thumbnail_url")
            .HasMaxLength(1000);

        builder.Property(m => m.ModuleConfig)
            .HasColumnName("module_config")
            .HasColumnType("jsonb");

        builder.Property(m => m.IsPublished)
            .HasColumnName("is_published")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(m => m.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(m => m.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(m => m.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(m => m.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(m => m.ModuleUuid)
            .IsUnique()
            .HasDatabaseName("uq_modules_module_uuid");

        builder.HasIndex(m => m.ModuleNameNormalized)
            .HasDatabaseName("idx_modules_module_name_normalized");

        builder.HasIndex(m => m.ModuleType)
            .HasDatabaseName("idx_modules_module_type");

        builder.HasIndex(m => m.DeletedAt)
            .HasDatabaseName("idx_modules_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(m => m.DeletedAt == null);
    }
}
