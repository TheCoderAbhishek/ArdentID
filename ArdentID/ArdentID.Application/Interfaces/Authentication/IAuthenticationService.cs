using ArdentID.Application.DTOs.Authentication;

namespace ArdentID.Application.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticationAsync(LoginRequestDto loginRequestDto);
        Task<(Guid, string)> RegisterUserAsync(UserRegistrationDto registrationDto);
    }
}
