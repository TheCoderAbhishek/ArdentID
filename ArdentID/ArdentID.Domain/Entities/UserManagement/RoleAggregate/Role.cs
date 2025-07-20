using ArdentID.Domain.Entities.UserManagement.JoinEntities;
using System.ComponentModel.DataAnnotations;

namespace ArdentID.Domain.Entities.UserManagement.RoleAggregate
{
    /// <summary>
    /// Represents a role that groups a set of permissions. Users are assigned roles.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique identifier for the role (Primary Key).
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The programmatic name of the role (e.g., "system_admin", "content_editor").
        /// Should be unique and not contain spaces.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        /// <summary>
        /// A user-friendly name for the role (e.g., "System Administrator").
        /// </summary>
        [Required]
        [MaxLength(150)]
        public required string DisplayName { get; set; }

        /// <summary>
        /// A detailed description of the role's purpose. This is optional.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// The UTC date and time the role was created.
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }

        // --- Navigation Properties ---
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
