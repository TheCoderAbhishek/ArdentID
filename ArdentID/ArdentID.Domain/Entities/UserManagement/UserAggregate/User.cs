using ArdentID.Domain.Entities.UserManagement.JoinEntities;
using System.ComponentModel.DataAnnotations;

namespace ArdentID.Domain.Entities.UserManagement.UserAggregate
{
    /// <summary>
    /// Represents a user account in the system. This is the central entity.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user (Primary Key).
        /// Using a Guid is the best practice for security and scalability.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The primary email address for the user, used for login and communication.
        /// This will be enforced as unique in the database configuration.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public required string Email { get; set; }

        /// <summary>
        /// A flag indicating if the user has confirmed their email address.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// The salted and hashed password for the user.
        /// This single string contains the hash, salt, and all other necessary info.
        /// </summary>
        [Required]
        [MaxLength(2048)]
        public required string PasswordHash { get; set; }

        /// <summary>
        /// A security stamp that is invalidated when a user's security-critical info changes.
        /// </summary>
        public string? SecurityStamp { get; set; }

        /// <summary>
        /// The user's given name (first name). Can be optional.
        /// </summary>
        [MaxLength(100)]
        public string? GivenName { get; set; }

        /// <summary>
        /// The user's family name (last name). Can be optional.
        /// </summary>
        [MaxLength(100)]
        public string? FamilyName { get; set; }

        /// <summary>
        /// The current status of the user account (e.g., "Active", "Suspended").
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "PendingVerification";

        /// <summary>
        /// The UTC date and time the user account was created.
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>
        /// The UTC date and time the user account was last updated.
        /// </summary>
        public DateTime UpdatedAtUtc { get; set; }

        /// <summary>
        /// The UTC date and time of the user's last login.
        /// </summary>
        public DateTime? LastLoginAtUtc { get; set; }

        // --- Navigation Properties ---
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
        public virtual ICollection<Session> Sessions { get; set; } = [];
    }
}
