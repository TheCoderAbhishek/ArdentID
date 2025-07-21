using ArdentID.Domain.Entities.UserManagement.UserAggregate;

namespace ArdentID.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<Guid> AddUserAsync(User user);
    }
}
