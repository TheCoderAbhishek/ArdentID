using ArdentID.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ArdentID.Application.DTOs.Verification
{
    public class VerifyOtpRequestDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public OtpPurpose Purpose { get; set; }

        [Required]
        public required string Token { get; set; }
    }
}
