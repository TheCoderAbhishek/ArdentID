using ArdentID.Domain.Entities.UserManagement.RoleAggregate;
using ArdentID.Domain.Entities.UserManagement.UserAggregate;

namespace ArdentID.Domain.Entities.UserManagement.JoinEntities
{
    /// <summary>
    /// Join table to link Users and Roles (Many-to-Many).
    /// </summary>
    public class UserRole
    {
        public Guid UserId { get; set; }
        public virtual required User User { get; set; }

        public Guid RoleId { get; set; }
        public virtual required Role Role { get; set; }

        /// <summary>
        /// The UTC date and time this role was assigned to the user.
        /// </summary>
        public DateTime AssignedAtUtc { get; set; }
    }
}
