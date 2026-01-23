using Escola.Domain.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Assets;

public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        // Table mapping
        builder.ToTable("media_files", "assets");

        // Primary key
        builder.HasKey(m => m.FileId);
        builder.Property(m => m.FileId)
            .HasColumnName("file_id")
            .ValueGeneratedOnAdd();

        // External UUID
        builder.Property(m => m.FileUuid)
            .HasColumnName("file_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(m => m.FileUuid)
            .IsUnique()
            .HasDatabaseName("uq_media_files_file_uuid");

        // Foreign Keys
        builder.Property(m => m.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(m => m.MimeTypeId)
            .HasColumnName("mime_type_id")
            .IsRequired();

        // Required Properties
        builder.Property(m => m.StoragePath)
            .HasColumnName("storage_path")
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.OriginalName)
            .HasColumnName("original_name")
            .IsRequired()
            .HasMaxLength(255);

        // Computed normalized column
        builder.Property(m => m.OriginalNameNormalized)
            .HasColumnName("original_name_normalized")
            .IsRequired()
            .HasMaxLength(255)
            .HasComputedColumnSql("immutable_unaccent(original_name)", stored: true);

        // Optional Properties
        builder.Property(m => m.SizeBytes)
            .HasColumnName("size_bytes");

        builder.Property(m => m.AltText)
            .HasColumnName("alt_text")
            .HasMaxLength(500);

        // Computed normalized column
        builder.Property(m => m.AltTextNormalized)
            .HasColumnName("alt_text_normalized")
            .HasMaxLength(500)
            .HasComputedColumnSql("immutable_unaccent(alt_text)", stored: true);

        builder.Property(m => m.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb");

        // Audit Properties
        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(m => m.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(m => m.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(m => m.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(m => m.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(m => m.Tenant)
            .WithMany()
            .HasForeignKey(m => m.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_media_files_tenant_id");

        builder.HasOne(m => m.MimeType)
            .WithMany()
            .HasForeignKey(m => m.MimeTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_media_files_mime_type_id");

        builder.HasOne(m => m.CreatedByUser)
            .WithMany()
            .HasForeignKey(m => m.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_media_files_created_by");

        builder.HasOne(m => m.UpdatedByUser)
            .WithMany()
            .HasForeignKey(m => m.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_media_files_updated_by");

        // Indexes
        builder.HasIndex(m => m.TenantId)
            .HasDatabaseName("idx_media_files_tenant_id");

        builder.HasIndex(m => m.MimeTypeId)
            .HasDatabaseName("idx_media_files_mime_type_id");

        builder.HasIndex(m => m.OriginalNameNormalized)
            .HasDatabaseName("idx_media_files_original_name_normalized");

        builder.HasIndex(m => new { m.DeletedAt, m.TenantId })
            .HasDatabaseName("idx_media_files_deleted_at_tenant_id");

        // Query filter for soft delete
        builder.HasQueryFilter(m => m.DeletedAt == null);
    }
}
