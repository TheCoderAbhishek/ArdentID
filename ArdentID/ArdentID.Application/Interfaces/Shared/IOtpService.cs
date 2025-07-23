namespace ArdentID.Application.Interfaces.Shared
{
    /// <summary>
    /// Defines the contract for a service that generates and validates One-Time Passwords (OTPs).
    /// </summary>
    public interface IOtpService
    {
        /// <summary>
        /// Generates a Time-based One-Time Password (TOTP) from a secret key.
        /// </summary>
        /// <param name="secretKey">The secret key shared between the server and the user.</param>
        /// <returns>A string representing the generated OTP (e.g., a 6-digit code).</returns>
        string GenerateOtp(byte[] secretKey);

        /// <summary>
        /// Validates a user-provided TOTP against a secret key.
        /// </summary>
        /// <param name="secretKey">The secret key used to generate the original OTP.</param>
        /// <param name="token">The OTP token provided by the user.</param>
        /// <returns>True if the token is valid within the current time window; otherwise, false.</returns>
        bool ValidateOtp(byte[] secretKey, string token);
    }
}
