using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Content;

public class UsageContextConfiguration : IEntityTypeConfiguration<Domain.Content.UsageContext>
{
    public void Configure(EntityTypeBuilder<Domain.Content.UsageContext> builder)
    {
        builder.ToTable("usage_contexts", "content");

        builder.HasKey(uc => uc.UsageContextId)
            .HasName("pk_usage_contexts");

        builder.Property(uc => uc.UsageContextId)
            .HasColumnName("usage_context_id")
            .UseIdentityAlwaysColumn();

        builder.Property(uc => uc.UsageContextUuid)
            .HasColumnName("usage_context_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(uc => uc.UsageContextUuid)
            .IsUnique()
            .HasDatabaseName("uq_usage_contexts_uuid");

        builder.Property(uc => uc.ContextCode)
            .HasColumnName("context_code")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(uc => uc.ContextCode)
            .IsUnique()
            .HasDatabaseName("uq_usage_contexts_code");

        builder.Property(uc => uc.ContextCodeNormalized)
            .HasColumnName("context_code_normalized")
            .HasMaxLength(100)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(context_code))", stored: true);

        builder.HasIndex(uc => uc.ContextCodeNormalized)
            .HasDatabaseName("idx_usage_contexts_code_normalized");

        builder.Property(uc => uc.ContextName)
            .HasColumnName("context_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(uc => uc.ContextNameNormalized)
            .HasColumnName("context_name_normalized")
            .HasMaxLength(200)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(context_name))", stored: true);

        builder.HasIndex(uc => uc.ContextNameNormalized)
            .HasDatabaseName("idx_usage_contexts_name_normalized");

        builder.Property(uc => uc.Description)
            .HasColumnName("description");

        builder.Property(uc => uc.IconName)
            .HasColumnName("icon_name")
            .HasMaxLength(100);

        builder.Property(uc => uc.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(uc => uc.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(uc => uc.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(uc => uc.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(uc => uc.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(uc => uc.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(uc => uc.DeletedAt)
            .HasDatabaseName("idx_usage_contexts_deleted_at")
            .HasFilter("deleted_at IS NULL");

        // Navigation
        builder.HasMany(uc => uc.ActivityResources)
            .WithOne(ar => ar.UsageContext)
            .HasForeignKey(ar => ar.UsageContextId)
            .HasConstraintName("fk_activity_resources_usage_context")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
