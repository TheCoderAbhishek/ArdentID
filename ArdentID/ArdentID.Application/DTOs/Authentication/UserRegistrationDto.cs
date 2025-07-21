using System.ComponentModel.DataAnnotations;

namespace ArdentID.Application.DTOs.Authentication
{
    public class UserRegistrationDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string GivenName { get; set; }

        [Required]
        public required string FamilyName { get; set; }
    }
}
