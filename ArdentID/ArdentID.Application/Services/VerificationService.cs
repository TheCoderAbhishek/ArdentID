using ArdentID.Application.DTOs.Verification;
using ArdentID.Application.Interfaces.Shared;
using ArdentID.Application.Interfaces.UserManagement;
using ArdentID.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;
using OtpNet;

namespace ArdentID.Application.Services
{
    public class VerificationService(IMemoryCache cache, IUserRepository userRepository, IOtpService otpService, IEmailService emailService) : IVerificationService
    {
        private readonly IMemoryCache _cache = cache;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IOtpService _otpService = otpService;
        private readonly IEmailService _emailService = emailService;

        public async Task<string> GenerateAndSendOtpAsync(GenerateOtpRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email) ?? throw new InvalidOperationException("User not found.");
            var secretKey = KeyGeneration.GenerateRandomKey(20);
            var cacheKey = $"otp_{request.Purpose}_{request.Email}";
            _cache.Set(cacheKey, secretKey, TimeSpan.FromMinutes(5));

            var token = _otpService.GenerateOtp(secretKey);

            string templateKey = request.Purpose switch
            {
                OtpPurpose.EmailConfirmation => "AccountActivation",
                OtpPurpose.PasswordReset => "PasswordReset",
                _ => throw new ArgumentOutOfRangeException(nameof(request.Purpose), "Invalid OTP purpose specified.")
            };

            var placeholders = new Dictionary<string, string>
            {
                { "UserName", user.GivenName ?? user.Email },
                { "Otp", token }
            };

            await _emailService.SendEmailAsync(user.Email, templateKey, placeholders);

            return $"A verification code for {request.Purpose} has been sent to your email.";
        }

        public async Task<bool> VerifyOtpAsync(VerifyOtpRequestDto request)
        {
            var cacheKey = $"otp_{request.Purpose}_{request.Email}";

            if (!_cache.TryGetValue(cacheKey, out byte[]? secretKey) || secretKey == null)
            {
                return false;
            }

            if (!_otpService.ValidateOtp(secretKey, request.Token))
            {
                return false;
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) return false;

            switch (request.Purpose)
            {
                case OtpPurpose.EmailConfirmation:
                    user.IsEmailConfirmed = true;
                    user.Status = "Active";
                    break;
                case OtpPurpose.PasswordReset:
                    break;
            }

            _cache.Remove(cacheKey);

            return true;
        }
    }
}
