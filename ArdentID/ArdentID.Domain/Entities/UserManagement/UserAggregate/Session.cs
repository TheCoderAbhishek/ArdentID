using System.ComponentModel.DataAnnotations;

namespace ArdentID.Domain.Entities.UserManagement.UserAggregate
{
    /// <summary>
    /// Represents a user's active session (e.g., a login instance).
    /// </summary>
    public class Session
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public virtual required User User { get; set; }

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        [MaxLength(512)]
        public string? UserAgent { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public DateTime? RevokedAtUtc { get; set; }
    }
}
