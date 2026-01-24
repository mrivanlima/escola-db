using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Content;

public class ModuleTypeConfiguration : IEntityTypeConfiguration<Domain.Content.ModuleType>
{
    public void Configure(EntityTypeBuilder<Domain.Content.ModuleType> builder)
    {
        builder.ToTable("module_types", "content");

        builder.HasKey(mt => mt.ModuleTypeId)
            .HasName("pk_module_types");

        builder.Property(mt => mt.ModuleTypeId)
            .HasColumnName("module_type_id")
            .UseIdentityAlwaysColumn();

        builder.Property(mt => mt.ModuleTypeUuid)
            .HasColumnName("module_type_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(mt => mt.ModuleTypeUuid)
            .IsUnique()
            .HasDatabaseName("uq_module_types_uuid");

        builder.Property(mt => mt.ModuleTypeCode)
            .HasColumnName("module_type_code")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(mt => mt.ModuleTypeCode)
            .IsUnique()
            .HasDatabaseName("uq_module_types_code");

        builder.Property(mt => mt.ModuleTypeCodeNormalized)
            .HasColumnName("module_type_code_normalized")
            .HasMaxLength(100)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(module_type_code))", stored: true);

        builder.HasIndex(mt => mt.ModuleTypeCodeNormalized)
            .HasDatabaseName("idx_module_types_code_normalized");

        builder.Property(mt => mt.ModuleTypeName)
            .HasColumnName("module_type_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(mt => mt.ModuleTypeNameNormalized)
            .HasColumnName("module_type_name_normalized")
            .HasMaxLength(200)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(module_type_name))", stored: true);

        builder.HasIndex(mt => mt.ModuleTypeNameNormalized)
            .HasDatabaseName("idx_module_types_name_normalized");

        builder.Property(mt => mt.Description)
            .HasColumnName("description");

        builder.Property(mt => mt.IconName)
            .HasColumnName("icon_name")
            .HasMaxLength(100);

        builder.Property(mt => mt.ColorCode)
            .HasColumnName("color_code")
            .HasMaxLength(20);

        builder.Property(mt => mt.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(mt => mt.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(mt => mt.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(mt => mt.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(mt => mt.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(mt => mt.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(mt => mt.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(mt => mt.DeletedAt)
            .HasDatabaseName("idx_module_types_deleted_at")
            .HasFilter("deleted_at IS NULL");

        // Navigation
        builder.HasMany(mt => mt.Modules)
            .WithOne(m => m.ModuleType)
            .HasForeignKey(m => m.ModuleTypeId)
            .HasConstraintName("fk_modules_module_type")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
