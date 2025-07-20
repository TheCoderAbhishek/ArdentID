using ArdentID.Domain.Entities.UserManagement.JoinEntities;
using System.ComponentModel.DataAnnotations;

namespace ArdentID.Domain.Entities.UserManagement.Shared
{
    /// <summary>
    /// Represents a single, granular permission in the system.
    /// This defines an action that can be performed on a resource.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Unique identifier for the permission (Primary Key).
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the resource this permission applies to (e.g., "Document", "UserAccount", "Dashboard").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public required string Resource { get; set; }

        /// <summary>
        /// The action that can be performed on the resource (e.g., "Create", "Read", "Update", "Delete", "Export").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public required string Action { get; set; }

        /// <summary>
        /// A user-friendly description of what this permission allows. This is optional.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        // --- Navigation Properties ---
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
