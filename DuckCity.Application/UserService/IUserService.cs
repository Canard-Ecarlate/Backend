using DuckCity.Domain.Users;

namespace DuckCity.Application.Services.Interfaces;

public interface IUserService
{
    public void DeleteAccountUser(string userId);

    public void ChangePasswordUser(string userId, string lastPassword, string newPassword,
        string newPasswordConfirmation);
}