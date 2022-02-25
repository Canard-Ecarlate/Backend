namespace DuckCity.Application.UserService;

public interface IUserService
{
    public void DeleteAccountUser(string userId);

    public void ChangePasswordUser(string userId, string actualPassword, string newPassword,
        string newPasswordConfirmation);
}