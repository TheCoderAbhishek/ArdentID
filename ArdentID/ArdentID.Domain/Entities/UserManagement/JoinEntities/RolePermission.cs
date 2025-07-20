using ArdentID.Domain.Entities.UserManagement.RoleAggregate;
using ArdentID.Domain.Entities.UserManagement.Shared;

namespace ArdentID.Domain.Entities.UserManagement.JoinEntities
{
    /// <summary>
    /// Join table to link Roles and Permissions (Many-to-Many).
    /// </summary>
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public virtual required Role Role { get; set; }

        public int PermissionId { get; set; }
        public virtual required Permission Permission { get; set; }

        /// <summary>
        /// The UTC date and time this permission was granted to the role.
        /// </summary>
        public DateTime GrantedAtUtc { get; set; }
    }
}
