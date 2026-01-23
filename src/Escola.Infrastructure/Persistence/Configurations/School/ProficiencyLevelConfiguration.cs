using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class ProficiencyLevelConfiguration : IEntityTypeConfiguration<ProficiencyLevel>
{
    public void Configure(EntityTypeBuilder<ProficiencyLevel> builder)
    {
        builder.ToTable("proficiency_levels", "school");

        builder.HasKey(p => p.ProficiencyLevelId)
            .HasName("pk_proficiency_levels");

        builder.Property(p => p.ProficiencyLevelId)
            .HasColumnName("proficiency_level_id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.ProficiencyUuid)
            .HasColumnName("proficiency_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(p => p.ProficiencyCode)
            .HasColumnName("proficiency_code")
            .IsRequired();

        builder.Property(p => p.ProficiencyCodeNormalized)
            .HasColumnName("proficiency_code_normalized")
            .IsRequired()
            .HasComputedColumnSql("public.immutable_unaccent(LOWER(TRIM(proficiency_code)))", stored: true);

        builder.Property(p => p.ProficiencyName)
            .HasColumnName("proficiency_name")
            .IsRequired();

        builder.Property(p => p.ProficiencyNameNormalized)
            .HasColumnName("proficiency_name_normalized")
            .IsRequired()
            .HasComputedColumnSql("public.immutable_unaccent(LOWER(TRIM(proficiency_name)))", stored: true);

        builder.Property(p => p.Description)
            .HasColumnName("description");

        builder.Property(p => p.MinimumYears)
            .HasColumnName("minimum_years");

        builder.Property(p => p.IconName)
            .HasColumnName("icon_name");

        builder.Property(p => p.ColorCode)
            .HasColumnName("color_code");

        builder.Property(p => p.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(p => p.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(p => p.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(p => p.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(p => p.Tenant)
            .WithMany()
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_proficiency_levels_tenant");

        builder.HasOne(p => p.CreatedByUser)
            .WithMany()
            .HasForeignKey(p => p.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_proficiency_levels_created");

        builder.HasOne(p => p.UpdatedByUser)
            .WithMany()
            .HasForeignKey(p => p.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_proficiency_levels_updated");

        builder.HasIndex(p => p.ProficiencyUuid)
            .IsUnique()
            .HasDatabaseName("uq_proficiency_levels_uuid");

        builder.HasIndex(p => new { p.TenantId, p.ProficiencyCode })
            .IsUnique()
            .HasDatabaseName("uq_proficiency_levels_code");

        builder.HasIndex(p => p.TenantId)
            .HasDatabaseName("idx_proficiency_levels_tenant");

        builder.HasIndex(p => p.DeletedAt)
            .HasDatabaseName("idx_proficiency_levels_deleted_at");

        builder.HasQueryFilter(p => p.DeletedAt == null);
    }
}
