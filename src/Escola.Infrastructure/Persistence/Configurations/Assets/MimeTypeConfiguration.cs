using Escola.Domain.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Assets;

public class MimeTypeConfiguration : IEntityTypeConfiguration<MimeType>
{
    public void Configure(EntityTypeBuilder<MimeType> builder)
    {
        builder.ToTable("mime_types", "assets");

        builder.HasKey(mt => mt.MimeTypeId)
            .HasName("pk_mime_types");

        builder.Property(mt => mt.MimeTypeId)
            .HasColumnName("mime_type_id")
            .ValueGeneratedOnAdd();

        builder.Property(mt => mt.MimeTypeUuid)
            .HasColumnName("mime_type_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(mt => mt.MimeTypeCode)
            .HasColumnName("mime_type_code")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mt => mt.MimeTypeName)
            .HasColumnName("mime_type_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(mt => mt.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder.Property(mt => mt.MimeTypeCodeNormalized)
            .HasColumnName("mime_type_code_normalized")
            .IsRequired()
            .HasComputedColumnSql("LOWER(immutable_unaccent(mime_type_code))", stored: true);

        builder.Property(mt => mt.FileExtension)
            .HasColumnName("file_extension")
            .HasMaxLength(20);

        builder.Property(mt => mt.IconName)
            .HasColumnName("icon_name")
            .HasMaxLength(100);

        builder.Property(mt => mt.MaxFileSizeMb)
            .HasColumnName("max_file_size_mb");

        builder.Property(mt => mt.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        // Foreign Keys
        builder.HasOne(mt => mt.Category)
            .WithMany()
            .HasForeignKey(mt => mt.CategoryId)
            .HasConstraintName("fk_mime_types_category")
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(mt => mt.MimeTypeUuid)
            .HasDatabaseName("uq_mime_types_uuid")
            .IsUnique();

        builder.HasIndex(mt => mt.MimeTypeCode)
            .HasDatabaseName("uq_mime_types_code")
            .IsUnique();

        builder.HasIndex(mt => mt.CategoryId)
            .HasDatabaseName("idx_mime_types_category");

        builder.HasIndex(mt => mt.MimeTypeCodeNormalized)
            .HasDatabaseName("idx_mime_types_code_normalized");
    }
}
