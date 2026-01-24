using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Game;

public class ProgressStatusConfiguration : IEntityTypeConfiguration<Domain.Game.ProgressStatus>
{
    public void Configure(EntityTypeBuilder<Domain.Game.ProgressStatus> builder)
    {
        builder.ToTable("progress_statuses", "game");

        builder.HasKey(ps => ps.StatusId)
            .HasName("pk_progress_statuses");

        builder.Property(ps => ps.StatusId)
            .HasColumnName("status_id")
            .UseIdentityAlwaysColumn();

        builder.Property(ps => ps.StatusUuid)
            .HasColumnName("status_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(ps => ps.StatusUuid)
            .IsUnique()
            .HasDatabaseName("uq_progress_statuses_uuid");

        builder.Property(ps => ps.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(ps => ps.StatusCode)
            .HasColumnName("status_code")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(ps => ps.StatusCode)
            .IsUnique()
            .HasDatabaseName("uq_progress_statuses_code");

        builder.Property(ps => ps.StatusCodeNormalized)
            .HasColumnName("status_code_normalized")
            .HasMaxLength(100)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(status_code))", stored: true);

        builder.HasIndex(ps => ps.StatusCodeNormalized)
            .HasDatabaseName("idx_progress_statuses_code_normalized");

        builder.Property(ps => ps.StatusName)
            .HasColumnName("status_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ps => ps.StatusNameNormalized)
            .HasColumnName("status_name_normalized")
            .HasMaxLength(200)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(status_name))", stored: true);

        builder.HasIndex(ps => ps.StatusNameNormalized)
            .HasDatabaseName("idx_progress_statuses_name_normalized");

        builder.Property(ps => ps.Description)
            .HasColumnName("description");

        builder.Property(ps => ps.IconName)
            .HasColumnName("icon_name")
            .HasMaxLength(100);

        builder.Property(ps => ps.ColorCode)
            .HasColumnName("color_code")
            .HasMaxLength(20);

        builder.Property(ps => ps.IsFinalState)
            .HasColumnName("is_final_state")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ps => ps.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(ps => ps.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(ps => ps.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(ps => ps.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(ps => ps.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(ps => ps.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(ps => ps.DeletedAt)
            .HasDatabaseName("idx_progress_statuses_deleted_at")
            .HasFilter("deleted_at IS NULL");

        // Navigation
        builder.HasOne(ps => ps.Tenant)
            .WithMany()
            .HasForeignKey(ps => ps.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_progress_statuses_tenant");

        builder.HasMany(ps => ps.StudentProgresses)
            .WithOne(sp => sp.ProgressStatus)
            .HasForeignKey(sp => sp.StatusId)
            .HasConstraintName("fk_student_progress_status")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
