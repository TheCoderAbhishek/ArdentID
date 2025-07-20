using ArdentID.Domain.Entities.UserManagement.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArdentID.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the many-to-many join table between Role and Permission.
    /// </summary>
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            // Define the composite primary key for the join table.
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            // Configure the many-to-one relationship from RolePermission to Role.
            builder.HasOne(rp => rp.Role)
                   .WithMany(r => r.RolePermissions)
                   .HasForeignKey(rp => rp.RoleId)
                   .OnDelete(DeleteBehavior.Cascade); // If a Role is deleted, its permission links are also deleted.

            // Configure the many-to-one relationship from RolePermission to Permission.
            builder.HasOne(rp => rp.Permission)
                   .WithMany(p => p.RolePermissions)
                   .HasForeignKey(rp => rp.PermissionId)
                   .OnDelete(DeleteBehavior.Cascade); // If a Permission is deleted, its links to roles are also deleted.
        }
    }
}
