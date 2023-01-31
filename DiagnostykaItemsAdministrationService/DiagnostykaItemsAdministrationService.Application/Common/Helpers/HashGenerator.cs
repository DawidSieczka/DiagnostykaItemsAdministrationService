using System.Security.Cryptography;
using System.Text;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers.Interfaces;

namespace DiagnostykaItemsAdministrationService.Application.Common.Helpers;

public class HashGenerator : IHashGenerator
{
    /// <summary>
    /// Generates a hash converted to string with fixed length of 12 characters.
    /// </summary>
    /// <param name="input">Value of type string.</param>
    /// <returns>hashed input converted to string.</returns>
    public string GenerateHash(string input)
    {
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hash).Substring(0, 12);
    }

    /// <summary>
    /// Generates a hash converted to string with fixed length of 12 characters.
    /// </summary>
    /// <param name="input">Value of type int.</param>
    /// <returns>hashed input converted to string.</returns>
    public string GenerateHash(int input) => GenerateHash(input.ToString());
}