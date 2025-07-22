using ArdentID.Application.DTOs.Authentication;
using ArdentID.Domain.Entities.UserManagement.UserAggregate;

namespace ArdentID.Application.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDto> AuthenticationAsync(LoginRequestDto loginRequestDto);
        Task<(Guid, string)> RegisterUserAsync(UserRegistrationDto registrationDto);
        Task<List<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllUsersEnumAsync();
    }
}
