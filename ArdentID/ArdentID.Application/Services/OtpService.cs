using ArdentID.Application.Interfaces.Shared;
using OtpNet;

namespace ArdentID.Application.Services
{
    /// <summary>
    /// Implements OTP generation and validation using the Otp.NET library.
    /// </summary>
    public class OtpService : IOtpService
    {
        // Define the parameters for our TOTP generation.
        private const int OtpStepSeconds = 300;
        private const int OtpSize = 6;

        /// <summary>
        /// Generates a 6-digit code that is valid for 5 minutes.
        /// </summary>
        public string GenerateOtp(byte[] secretKey)
        {
            // Create a new TOTP instance with the specified parameters.
            var otp = new Totp(secretKey, step: OtpStepSeconds, totpSize: OtpSize);

            // Compute the TOTP for the current time.
            return otp.ComputeTotp();
        }

        /// <summary>
        /// Validates the user's token, allowing for a small time drift between server and client.
        /// </summary>
        public bool ValidateOtp(byte[] secretKey, string token)
        {
            try
            {
                // Create a TOTP instance with the exact same parameters used for generation.
                var otp = new Totp(secretKey, step: OtpStepSeconds, totpSize: OtpSize);

                // Verify the token. The VerificationWindow allows the token to be valid
                // even if there's a slight clock skew between the server and the client's device,
                // which is a recommended practice.
                return otp.VerifyTotp(token, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
            }
            catch
            {
                // If any error occurs during verification (e.g., invalid token format),
                // we treat it as a failed validation for security.
                return false;
            }
        }
    }
}
