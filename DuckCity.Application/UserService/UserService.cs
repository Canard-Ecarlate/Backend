using DuckCity.Application.Services.Interfaces;
using DuckCity.Application.Utils;
using DuckCity.Application.Validations;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.UserRepository;

namespace DuckCity.Application.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void DeleteAccountUser(string userId)
    {
        CheckValid.ExistUser(_userRepository, userId);
        _userRepository.DeleteUserById(userId);
    }

    public void ChangePasswordUser(string userId, string actualPassword, string newPassword,
        string newPasswordConfirmation)
    {
        if (newPassword != newPasswordConfirmation)
        {
            throw new PasswordConfirmationException();
        }
        CheckValid.ExistUser(_userRepository, userId);
        User user = _userRepository.FindById(userId ?? throw new BadUserOrPasswordException());
        UserUtils.ComparePassword(user, actualPassword);
        string encryptedPassword = UserUtils.HashPassword(newPassword);
        user.Password = encryptedPassword;
        _userRepository.Replace(user);
    }
}