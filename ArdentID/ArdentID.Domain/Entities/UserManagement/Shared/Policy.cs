using System.ComponentModel.DataAnnotations;

namespace ArdentID.Domain.Entities.UserManagement.Shared
{
    /// <summary>
    /// Represents a dynamic, attribute-based access control (ABAC) policy.
    /// Policies add conditional logic on top of role-based permissions.
    /// </summary>
    public class Policy
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        /// <summary>
        /// A detailed description of the policy's purpose. This is optional.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// The effect of the policy, either "Allow" or "Deny".
        /// Deny rules should always override Allow rules.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Effect { get; set; } = "Allow";

        /// <summary>
        /// The set of rules for this policy, stored as a JSON string.
        /// This provides maximum flexibility to define complex conditions
        /// (e.g., based on time, location, resource attributes) without changing the schema.
        /// Example: {"timeOfDay": {"start": "09:00", "end": "17:00"}, "ipRange": ["192.168.1.0/24"]}
        /// </summary>
        [Required]
        public required string RulesJson { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
