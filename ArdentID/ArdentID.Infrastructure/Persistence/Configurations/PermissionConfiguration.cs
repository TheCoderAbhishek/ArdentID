using ArdentID.Domain.Entities.UserManagement.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArdentID.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Permission entity.
    /// </summary>
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);

            // A permission should be uniquely defined by the combination of its resource and action.
            // This prevents creating duplicate permissions like ('Users', 'Create') twice.
            builder.HasIndex(p => new { p.Resource, p.Action }).IsUnique();

            builder.Property(p => p.Resource)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Action)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
