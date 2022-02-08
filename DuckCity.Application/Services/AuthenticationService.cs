using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Application.Services.Utils;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace DuckCity.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
        
    public User Login(string? name, string? password)
    {
        IList<User> users = _userRepository.GetByName(name);

        if (users.Count != 1)
        {
            throw new BadUserOrPasswordException();
        }
            
        User user = users[0];
        UserUtils.ComparePassword(user,password);
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
                    string encryptedPassword = UserUtils.HashPassword(password);
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

        string strKey = Environment.GetEnvironmentVariable("SIGNATURE_KEY") ?? throw new InvalidOperationException();
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(strKey));
        
        JwtSecurityToken token = new(
            "https://canardecarlate.fr",
            "https://canardecarlate.fr",
            claims,
            expires: DateTime.Now.AddDays(30.0),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
            
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}