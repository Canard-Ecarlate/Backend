using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DuckCity.Application.Utils;
using DuckCity.Application.Validations;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.UserRepository;
using Microsoft.IdentityModel.Tokens;

namespace DuckCity.Application.AuthenticationService;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User Login(string? name, string? password)
    {
        try
        {
            User user = _userRepository.FindByName(name);
            UserUtils.ComparePassword(user, password);
            return user;
        }
        catch
        {
            throw new BadUserOrPasswordException();
        }
    }

    public void SignUp(string? name, string? email, string? password, string? passwordConfirmation)
    {
        CheckValid.SignUp(_userRepository, name, email, password, passwordConfirmation);
        string encryptedPassword = UserUtils.HashPassword(password);
        User user = new(name, email, encryptedPassword);
        _userRepository.Create(user);
    }

    public string GenerateJsonWebToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim("userId", user.Id!),
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