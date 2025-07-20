using ArdentID.Domain.Entities.UserManagement.RoleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArdentID.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasIndex(r => r.Name).IsUnique();
            builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
            builder.Property(r => r.DisplayName).IsRequired().HasMaxLength(150);
        }
    }
}
