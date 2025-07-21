using ArdentID.Application.DTOs.Authentication;

namespace ArdentID.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(Guid, string)> RegisterUserAsync(UserRegistrationDto registrationDto);
    }
}
