namespace ArdentID.Application.Interfaces
{
    public interface IPasswordService
    {
        /// <summary>
        /// Hashes a plain-text password using Argon2id.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>A secure, combined hash string containing the salt.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a plain-text password against a stored hash.
        /// </summary>
        /// <param name="hash">The stored hash from the database.</param>
        /// <param name="password">The plain-text password to verify.</param>
        /// <returns>True if the password is correct; otherwise, false.</returns>
        bool VerifyPassword(string hash, string password);
    }
}
