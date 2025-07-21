using ArdentID.Application.DTOs.Authentication;
using ArdentID.Application.Interfaces;
using ArdentID.Domain.Entities.UserManagement.UserAggregate;
using Microsoft.Extensions.Logging;

namespace ArdentID.Application.Services
{
    public class AuthenticationService(ILogger<AuthenticationService> logger, IPasswordService passwordService, IUserRepository userRepository) : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger = logger;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IUserRepository _userRepository = userRepository;

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
                    _logger.LogError($"A user with this email address already exists. {registrationDto.Email}");
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
                    Status = "Active",
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow
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

        /// <summary>
        /// This endpoint for testing Argon2 working as expected or not.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns>Return true or false.</returns>
        public async Task<bool> PasswordVerifyAsync(string email, string plainText)
        {
            try
            {
                // 1. Find the user by their email address.
                var user = await _userRepository.GetByEmailAsync(email);

                // 2. Check if the user exists. If not, verification fails.
                if (user == null)
                {
                    _logger.LogWarning("Login attempt for non-existent email: {Email}", email);
                    return false;
                }

                // 3. Use the password service to securely verify the password.
                bool isPasswordCorrect = _passwordService.VerifyPassword(user.PasswordHash, plainText);

                if (!isPasswordCorrect)
                {
                    _logger.LogWarning("Failed login attempt for user: {Email}", email);
                    return false; // Password does not match.
                }

                // 4. If password is correct, return the user object.
                _logger.LogInformation("User {Email} logged in successfully.", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for email {Email}", email);
                throw;
            }
        }
    }
}
