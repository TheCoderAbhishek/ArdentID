using ArdentID.Application.DTOs.Verification;

namespace ArdentID.Application.Interfaces.Shared
{
    public interface IVerificationService
    {
        /// <summary>
        /// Generates an OTP, caches the secret, and sends it to the user.
        /// </summary>
        Task<string> GenerateAndSendOtpAsync(GenerateOtpRequestDto request);

        /// <summary>
        /// Verifies an OTP and performs the action associated with its purpose.
        /// </summary>
        Task<bool> VerifyOtpAsync(VerifyOtpRequestDto request);
    }
}
