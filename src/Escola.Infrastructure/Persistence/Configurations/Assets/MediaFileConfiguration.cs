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

        // Properties
        builder.Property(m => m.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(m => m.StoragePath)
            .HasColumnName("storage_path")
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.OriginalName)
            .HasColumnName("original_name")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.OriginalNameNormalized)
            .HasColumnName("original_name_normalized")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.MimeType)
            .HasColumnName("mime_type")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.SizeBytes)
            .HasColumnName("size_bytes")
            .IsRequired();

        builder.Property(m => m.AltText)
            .HasColumnName("alt_text")
            .HasMaxLength(500);

        builder.Property(m => m.AltTextNormalized)
            .HasColumnName("alt_text_normalized")
            .HasMaxLength(500);

        builder.Property(m => m.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb");

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

        // Relationships
        builder.HasOne(m => m.Tenant)
            .WithMany()
            .HasForeignKey(m => m.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_media_files_tenant_id");

        // Indexes
        builder.HasIndex(m => m.TenantId)
            .HasDatabaseName("idx_media_files_tenant_id");

        builder.HasIndex(m => m.OriginalNameNormalized)
            .HasDatabaseName("idx_media_files_original_name_normalized");

        builder.HasIndex(m => m.DeletedAt)
            .HasDatabaseName("idx_media_files_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(m => m.DeletedAt == null);
    }
}
