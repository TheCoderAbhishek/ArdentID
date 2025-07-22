using ArdentID.Application.Interfaces.Shared;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace ArdentID.Application.Services
{
    /// <summary>
    /// Provides services for securely hashing and verifying passwords using the Argon2id algorithm.
    /// </summary>
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Hashes a plaintext password using Argon2id with a randomly generated salt.
        /// </summary>
        /// <param name="password">The plaintext password to hash.</param>
        /// <returns>A Base64 encoded string representing the combined salt and hash, ready for storage.</returns>
        public string HashPassword(string password)
        {
            // 1. Generate a secure, random salt
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // 2. Hash the password with Argon2id
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // 8 cores
                Iterations = 4,         // 4 rounds
                MemorySize = 1024 * 128 // 128 MB
            };

            var hash = argon2.GetBytes(32); // 32-byte hash

            // 3. Combine salt and hash, then encode to a single string for storage
            var combinedBytes = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, combinedBytes, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, combinedBytes, salt.Length, hash.Length);

            return Convert.ToBase64String(combinedBytes);
        }

        /// <summary>
        /// Verifies if a plaintext password matches a stored hash.
        /// </summary>
        /// <remarks>
        /// This method extracts the salt from the stored hash, re-hashes the provided password with the same parameters,
        /// and performs a constant-time comparison to prevent timing attacks.
        /// </remarks>
        /// <param name="hash">The stored, Base64 encoded hash string (which includes the salt).</param>
        /// <param name="password">The plaintext password to verify.</param>
        /// <returns><c>true</c> if the password matches the hash; otherwise, <c>false</c>.</returns>
        public bool VerifyPassword(string hash, string password)
        {
            try
            {
                // 1. Decode the stored hash string to get the original salt and hash
                var combinedBytes = Convert.FromBase64String(hash);
                var salt = new byte[16];
                var originalHash = new byte[32];
                Buffer.BlockCopy(combinedBytes, 0, salt, 0, salt.Length);
                Buffer.BlockCopy(combinedBytes, salt.Length, originalHash, 0, originalHash.Length);

                // 2. Hash the incoming password using the *exact same* salt and parameters
                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = 8,
                    Iterations = 4,
                    MemorySize = 1024 * 128
                };

                var newHash = argon2.GetBytes(32);

                // 3. Compare the hashes in a way that prevents timing attacks
                return CryptographicOperations.FixedTimeEquals(originalHash, newHash);
            }
            catch
            {
                // If any error occurs (e.g., invalid Base64 string), verification fails.
                return false;
            }
        }
    }
}
