using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Content;

public class ActivityTypeConfiguration : IEntityTypeConfiguration<Domain.Content.ActivityType>
{
    public void Configure(EntityTypeBuilder<Domain.Content.ActivityType> builder)
    {
        builder.ToTable("activity_types", "content");

        builder.HasKey(at => at.ActivityTypeId)
            .HasName("pk_activity_types");

        builder.Property(at => at.ActivityTypeId)
            .HasColumnName("activity_type_id")
            .UseIdentityAlwaysColumn();

        builder.Property(at => at.ActivityTypeUuid)
            .HasColumnName("activity_type_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(at => at.ActivityTypeUuid)
            .IsUnique()
            .HasDatabaseName("uq_activity_types_uuid");

        builder.Property(at => at.ActivityTypeCode)
            .HasColumnName("activity_type_code")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(at => at.ActivityTypeCode)
            .IsUnique()
            .HasDatabaseName("uq_activity_types_code");

        builder.Property(at => at.ActivityTypeCodeNormalized)
            .HasColumnName("activity_type_code_normalized")
            .HasMaxLength(100)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(activity_type_code))", stored: true);

        builder.HasIndex(at => at.ActivityTypeCodeNormalized)
            .HasDatabaseName("idx_activity_types_code_normalized");

        builder.Property(at => at.ActivityTypeName)
            .HasColumnName("activity_type_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(at => at.ActivityTypeNameNormalized)
            .HasColumnName("activity_type_name_normalized")
            .HasMaxLength(200)
            .IsRequired()
            .HasComputedColumnSql("LOWER(IMMUTABLE_UNACCENT(activity_type_name))", stored: true);

        builder.HasIndex(at => at.ActivityTypeNameNormalized)
            .HasDatabaseName("idx_activity_types_name_normalized");

        builder.Property(at => at.Description)
            .HasColumnName("description");

        builder.Property(at => at.IconName)
            .HasColumnName("icon_name")
            .HasMaxLength(100);

        builder.Property(at => at.DefaultPoints)
            .HasColumnName("default_points");

        builder.Property(at => at.RequiresInteraction)
            .HasColumnName("requires_interaction")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(at => at.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(at => at.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(at => at.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(at => at.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(at => at.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(at => at.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(at => at.DeletedAt)
            .HasDatabaseName("idx_activity_types_deleted_at")
            .HasFilter("deleted_at IS NULL");

        // Navigation
        builder.HasMany(at => at.Activities)
            .WithOne(a => a.ActivityType)
            .HasForeignKey(a => a.ActivityTypeId)
            .HasConstraintName("fk_activities_activity_type")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
