using DuckCity.Domain.Users;

namespace DuckCity.Application.AuthenticationService;

public interface IAuthenticationService
{
    User Login(string name, string password);
    
    void SignUp(string name, string email, string password, string passwordConfirmation);

    string GenerateJsonWebToken(User user);
}