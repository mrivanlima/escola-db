using Escola.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infrastructure.Persistence.Configurations.Identity;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        // Table mapping
        builder.ToTable("app_users", "identity");

        // Primary key
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(u => u.UserUuid)
            .HasColumnName("user_uuid")
            .IsRequired();

        builder.Property(u => u.TenantId)
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(u => u.AuthUserId)
            .HasColumnName("auth_user_id")
            .IsRequired();

        builder.Property(u => u.FullName)
            .HasColumnName("full_name")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.FullNameNormalized)
            .HasColumnName("full_name_normalized")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.EmailNormalized)
            .HasColumnName("email_normalized")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.UserRole)
            .HasColumnName("user_role")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.UserConfig)
            .HasColumnName("user_config")
            .HasColumnType("jsonb");

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(u => u.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(u => u.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne(u => u.Tenant)
            .WithMany(t => t.AppUsers)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_app_users_tenant_id");

        // Indexes
        builder.HasIndex(u => u.UserUuid)
            .IsUnique()
            .HasDatabaseName("uq_app_users_user_uuid");

        builder.HasIndex(u => u.AuthUserId)
            .IsUnique()
            .HasDatabaseName("uq_app_users_auth_user_id");

        builder.HasIndex(u => u.TenantId)
            .HasDatabaseName("idx_app_users_tenant_id");

        builder.HasIndex(u => u.EmailNormalized)
            .HasDatabaseName("idx_app_users_email_normalized");

        builder.HasIndex(u => u.DeletedAt)
            .HasDatabaseName("idx_app_users_deleted_at");

        // Query filter for soft delete
        builder.HasQueryFilter(u => u.DeletedAt == null);
    }
}
