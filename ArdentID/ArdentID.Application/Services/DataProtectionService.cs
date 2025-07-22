using ArdentID.Application.Interfaces.Shared;
using Microsoft.AspNetCore.DataProtection;

namespace ArdentID.Application.Services
{
    /// <summary>
    /// Data Protection Service.
    /// </summary>
    /// <param name="provider"></param>
    public class DataProtectionService(IDataProtectionProvider provider) : IDataProtectionService
    {
        private readonly IDataProtector _protector = provider.CreateProtector("ArdentID.SensitiveData.v1");

        /// <summary>
        /// Encrypts a plaintext string.
        /// </summary>
        /// <param name="plainText">The string to encrypt.</param>
        /// <returns>A protected, encrypted string that is safe for storage.</returns>
        public string Encrypt(string plainText)
        {
            return _protector.Protect(plainText);
        }

        /// <summary>
        /// Decrypts a protected string back to its original plaintext.
        /// </summary>
        /// <param name="cipherText">The protected string to decrypt.</param>
        /// <returns>The original, decrypted plaintext string.</returns>
        /// <exception cref="CryptographicException">Thrown if the cipher text is invalid, has been tampered with, or cannot be decrypted.</exception>
        public string Decrypt(string cipherText)
        {
            return _protector.Unprotect(cipherText);
        }
    }
}
