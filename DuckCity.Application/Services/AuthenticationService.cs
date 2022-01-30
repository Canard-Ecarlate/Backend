using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DuckCity.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private const int HashSize = 16;
    private const int Iterations = 100000;
    private const int ByteSize = 20;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
        
    public User Login(string? name, string? password)
    {
        IList<User> users = _userRepository.GetByName(name);

        if (users.Count != 1)
        {
            throw new UnauthorizedAccessException();
        }
            
        User user = users[0];
        /* Extract the bytes */
        byte[] hashBytes = Convert.FromBase64String(user.Password ?? throw new UnauthorizedAccessException());
        /* Get the salt */
        byte[] salt = new byte[HashSize];
        Array.Copy(hashBytes, 0, salt, 0, HashSize);
        /* Compute the hash on the password the user entered */
        Rfc2898DeriveBytes pbkdf2 = new(password ?? throw new UnauthorizedAccessException(), salt, Iterations);
        byte[] hash = pbkdf2.GetBytes(ByteSize);
        /* Compare the results */
        for (int i = 0; i < ByteSize; i++)
        {
            if (hashBytes[i + HashSize] != hash[i])
            {
                throw new UnauthorizedAccessException();
            }
        }
        return user;
    }

    public void SignUp(string? name, string? email, string? password, string? passwordConfirmation)
    {
        if (_userRepository.CountUserByName(name) == 0)
        {
            if (_userRepository.CountUserByEmail(email) == 0)
            {
                if (password == passwordConfirmation)
                {
                    string encryptedPassword = HashPassword(password);
                    User user = new User {Name = name, Email = email, Password = encryptedPassword};
                    _userRepository.Create(user);
                }
                else
                {
                    throw new PasswordConfirmationException();
                }
            }
            else
            {
                throw new MailAlreadyExistException(email);
            }
        }
        else
        {
            throw new UsernameAlreadyExistException(name);
        }
    }

    public string GenerateJsonWebToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim("userId",user.Id!),
            new Claim("type", "player")
        };

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

        JwtSecurityToken token = new(
            "https://canardecarlate.fr",
            "https://canardecarlate.fr",
            claims,
            expires: DateTime.Now.AddDays(30.0),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
            
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private static string HashPassword(string? password)
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
}