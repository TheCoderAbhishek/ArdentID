using ArdentID.Domain.Entities.UserManagement.UserAggregate;

namespace ArdentID.Application.Interfaces.UserManagement
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<Guid> AddUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllUsersEnumAsync();
    }
}
