using System.Security.Cryptography;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Users;

namespace DuckCity.Application.Services.Utils;

public class UserUtils
{
    private const int HashSize = 16;
    private const int Iterations = 100000;
    private const int ByteSize = 20;

    public static string HashPassword(string? password)
    {
        byte[] salt = new byte[HashSize];
        RandomNumberGenerator.Create().GetBytes(salt);
            
        Rfc2898DeriveBytes pbkdf2 = new(password ?? throw new ArgumentNullException(nameof(password)), salt, Iterations);
        byte[] hash = pbkdf2.GetBytes(ByteSize);
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, HashSize);
        Array.Copy(hash, 0, hashBytes, HashSize, ByteSize);
        string savedPasswordHash = Convert.ToBase64String(hashBytes);
        return savedPasswordHash;
    }

    public static void ComparePassword(User user,string? password)
    {
        /* Extract the bytes */
        byte[] hashBytes = Convert.FromBase64String(user.Password ?? throw new BadUserOrPasswordException());
        /* Get the salt */
        byte[] salt = new byte[HashSize];
        Array.Copy(hashBytes, 0, salt, 0, HashSize);
        /* Compute the hash on the password the user entered */
        Rfc2898DeriveBytes pbkdf2 = new(password ?? throw new BadUserOrPasswordException(), salt, Iterations);
        byte[] hash = pbkdf2.GetBytes(ByteSize);
        /* Compare the results */
        for (int i = 0; i < ByteSize; i++)
        {
            if (hashBytes[i + HashSize] != hash[i])
            {
                throw new BadUserOrPasswordException();
            }
        }
    }
}