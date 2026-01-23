using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.School;

public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.ToTable("specializations", "school");

        builder.HasKey(s => s.SpecializationId);
        builder.Property(s => s.SpecializationId)
            .HasColumnName("specialization_id")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.SpecializationUuid)
            .HasColumnName("specialization_uuid")
            .IsRequired();

        builder.Property(s => s.SpecializationName)
            .HasColumnName("specialization_name")
            .IsRequired();

        builder.Property(s => s.SpecializationNameNormalized)
            .HasColumnName("specialization_name_normalized")
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnName("description");

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(s => s.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(s => s.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(s => s.CreatedByUser)
            .WithMany()
            .HasForeignKey(s => s.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_specializations_created");

        builder.HasOne(s => s.UpdatedByUser)
            .WithMany()
            .HasForeignKey(s => s.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_specializations_updated");

        builder.HasIndex(s => s.SpecializationUuid)
            .IsUnique()
            .HasDatabaseName("uq_specializations_uuid");

        builder.HasIndex(s => s.SpecializationNameNormalized)
            .HasDatabaseName("idx_specializations_name_normalized");

        builder.HasIndex(s => s.DeletedAt)
            .HasDatabaseName("idx_specializations_deleted_at");

        builder.HasQueryFilter(s => s.DeletedAt == null);
    }
}
