using ArdentID.Application.DTOs.Authentication;
using ArdentID.Application.Interfaces;
using ArdentID.Domain.Entities.UserManagement.UserAggregate;

namespace ArdentID.Application.Services
{
    public class AuthenticationService(IUserRepository userRepository) : IAuthenticationService
    {
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
                    throw new InvalidOperationException("A user with this email address already exists.");
                }

                // 2. Hash the password.
                string passwordHash = "PLACEHOLDER_HASH";

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
                throw;
            }
        }
    }
}
