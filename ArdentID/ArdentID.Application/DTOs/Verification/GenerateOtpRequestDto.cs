using ArdentID.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ArdentID.Application.DTOs.Verification
{
    public class GenerateOtpRequestDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public OtpPurpose Purpose { get; set; }
    }
}
