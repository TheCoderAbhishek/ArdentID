using ArdentID.Application.DTOs.Authentication;
using ArdentID.Application.Interfaces.Authentication;
using ArdentID.Application.Interfaces.Shared;
using ArdentID.Application.Interfaces.UserManagement;
using ArdentID.Domain.Entities.UserManagement.UserAggregate;
using Microsoft.Extensions.Logging;

namespace ArdentID.Application.Services
{
    public class AuthenticationService(ILogger<AuthenticationService> logger, IPasswordService passwordService, IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator) : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger = logger;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

        /// <summary>
        /// This endpoint for testing Argon2 working as expected or not.
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns>Return true or false.</returns>
        public async Task<LoginResponseDto> AuthenticationAsync(LoginRequestDto loginRequestDto)
        {
            var response = new LoginResponseDto
            {
                Id = Guid.Empty,
                Result = false,
                JwtToken = string.Empty
            };

            try
            {
                var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt for non-existent email: {Email}", loginRequestDto.Email);
                    return response;
                }

                bool isPasswordCorrect = _passwordService.VerifyPassword(user.PasswordHash, loginRequestDto.Password);
                if (!isPasswordCorrect)
                {
                    _logger.LogWarning("Failed login attempt for user: {Email}", loginRequestDto.Email);
                    return response;
                }

                response.Id = user.Id;
                response.Result = true;
                response.JwtToken = _jwtTokenGenerator.GenerateToken(user);

                _logger.LogInformation("User {Email} logged in successfully.", loginRequestDto.Email);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for email {Email}", loginRequestDto.Email);
                throw;
            }
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registrationDto">The user registration data.</param>
        /// <returns>A tuple containing the new user's ID and a success message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a user with the same email already exists.</exception>
        public async Task<(Guid, string)> RegisterUserAsync(UserRegistrationDto registrationDto)
        {
            try
            {
                // 1. Check if a user with the given email already exists.
                var existingUser = await _userRepository.GetByEmailAsync(registrationDto.Email);
                if (existingUser != null)
                {
                    _logger.LogError("A user with this email address already exists. {Email}", registrationDto.Email);
                    throw new InvalidOperationException("A user with this email address already exists.");
                }

                // 2. Hash the password.
                string passwordHash = _passwordService.HashPassword(registrationDto.Password);

                // 3. Create a new User entity instance.
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = registrationDto.Email,
                    PasswordHash = passwordHash,
                    GivenName = registrationDto.GivenName,
                    FamilyName = registrationDto.FamilyName,
                    IsEmailConfirmed = false,
                    Status = "PendingVerification",
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow,
                    AccessFailedCount = 0,
                    LockoutEndUtc = null
                };

                // 4. Add the new user to the repository.
                Guid userId = await _userRepository.AddUserAsync(newUser);

                // 6. Return the result.
                return (userId, "User registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for email {Email}", registrationDto.Email);
                throw;
            }
        }
    }
}
