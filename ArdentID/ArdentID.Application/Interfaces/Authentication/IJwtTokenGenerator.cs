using ArdentID.Domain.Entities.UserManagement.UserAggregate;

namespace ArdentID.Application.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT for a given user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>The JWT string.</returns>
        string GenerateToken(User user);
    }
}
