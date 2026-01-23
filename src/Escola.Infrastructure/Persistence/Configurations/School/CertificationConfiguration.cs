using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class CertificationConfiguration : IEntityTypeConfiguration<Certification>
{
    public void Configure(EntityTypeBuilder<Certification> builder)
    {
        builder.ToTable("certifications", "school");

        builder.HasKey(c => c.CertificationId)
            .HasName("pk_certifications");

        builder.Property(c => c.CertificationId)
            .HasColumnName("certification_id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.CertificationUuid)
            .HasColumnName("certification_uuid")
            .IsRequired()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.CertificationName)
            .HasColumnName("certification_name")
            .IsRequired();

        builder.Property(c => c.CertificationNameNormalized)
            .HasColumnName("certification_name_normalized")
            .IsRequired()
            .HasComputedColumnSql("LOWER(immutable_unaccent(certification_name))", stored: true);

        builder.Property(c => c.Description)
            .HasColumnName("description");

        builder.Property(c => c.IssuingOrganization)
            .HasColumnName("issuing_organization");

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(c => c.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(c => c.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(c => c.CreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_certifications_created");

        builder.HasOne(c => c.UpdatedByUser)
            .WithMany()
            .HasForeignKey(c => c.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_certifications_updated");

        builder.HasIndex(c => c.CertificationUuid)
            .IsUnique()
            .HasDatabaseName("uq_certifications_uuid");

        builder.HasIndex(c => c.CertificationName)
            .IsUnique()
            .HasDatabaseName("uq_certifications_name");

        builder.HasIndex(c => c.CertificationNameNormalized)
            .HasDatabaseName("idx_certifications_name_normalized");

        builder.HasIndex(c => c.DeletedAt)
            .HasDatabaseName("idx_certifications_deleted_at");

        builder.HasQueryFilter(c => c.DeletedAt == null);
    }
}
