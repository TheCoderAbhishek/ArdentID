using ArdentID.Application.Interfaces.UserManagement;
using ArdentID.Domain.Entities.UserManagement.UserAggregate;
using ArdentID.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ArdentID.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implements the IUserRepository interface to provide data access logic for User entities.
    /// </summary>
    /// <param name="context">The database context, injected via DI.</param>
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        /// <summary>
        /// Retrieves a user from the database by their email address.
        /// </summary>
        /// <param name="email">The email of the user to find.</param>
        /// <returns>The User entity if found; otherwise, null.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Adds a new user entity to the database context.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>The ID of the newly added user.</returns>
        /// <remarks>
        /// This method only marks the entity for addition in the change tracker.
        /// </remarks>
        public async Task<Guid> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
