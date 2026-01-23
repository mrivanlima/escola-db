using Escola.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Identity;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Table mapping
        builder.ToTable("user_roles", "identity");

        // Primary key
        builder.HasKey(ur => ur.UserRoleId);
        builder.Property(ur => ur.UserRoleId)
            .HasColumnName("user_role_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(ur => ur.UserRoleUuid)
            .HasColumnName("user_role_uuid")
            .IsRequired();

        builder.Property(ur => ur.RoleName)
            .HasColumnName("role_name")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ur => ur.RoleNameNormalized)
            .HasColumnName("role_name_normalized")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ur => ur.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

        builder.Property(ur => ur.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ur => ur.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(ur => ur.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(ur => ur.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(ur => ur.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(ur => ur.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(ur => ur.UserRoleUuid)
            .IsUnique()
            .HasDatabaseName("uq_user_roles_user_role_uuid");

        builder.HasIndex(ur => ur.RoleNameNormalized)
            .HasDatabaseName("idx_user_roles_role_name_normalized");

        builder.HasIndex(ur => ur.DeletedAt)
            .HasDatabaseName("idx_user_roles_deleted_at");

        // Relationships
        builder.HasOne(ur => ur.Creator)
            .WithMany()
            .HasForeignKey(ur => ur.CreatedBy)
            .HasConstraintName("fk_user_roles_created_by")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Updater)
            .WithMany()
            .HasForeignKey(ur => ur.UpdatedBy)
            .HasConstraintName("fk_user_roles_updated_by")
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(ur => ur.DeletedAt == null);
    }
}
