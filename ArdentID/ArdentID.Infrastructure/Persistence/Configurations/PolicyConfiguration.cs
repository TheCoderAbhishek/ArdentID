using ArdentID.Domain.Entities.UserManagement.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArdentID.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Policy entity.
    /// </summary>
    public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.HasKey(p => p.Id);

            // Policy names should be unique for easy reference.
            builder.HasIndex(p => p.Name).IsUnique();

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.Effect)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(p => p.RulesJson)
                   .IsRequired();
        }
    }
}
