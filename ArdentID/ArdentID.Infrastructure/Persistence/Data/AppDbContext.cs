using ArdentID.Domain.Entities.UserManagement.JoinEntities;
using ArdentID.Domain.Entities.UserManagement.RoleAggregate;
using ArdentID.Domain.Entities.UserManagement.Shared;
using ArdentID.Domain.Entities.UserManagement.UserAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ArdentID.Infrastructure.Persistence.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
