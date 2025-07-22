namespace ArdentID.Application.Interfaces.Shared
{
    public interface IDataProtectionService
    {
        /// <summary>
        /// Encrypts a plain-text string.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <returns>A protected, encrypted string.</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts a protected string back to plain text.
        /// </summary>
        /// <param name="cipherText">The encrypted text to decrypt.</param>
        /// <returns>The original plain text.</returns>
        string Decrypt(string cipherText);
    }
}
