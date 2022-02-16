namespace DuckCity.Application.UserService;

public interface IUserService
{
    public void DeleteAccountUser(string userId);

    public void ChangePasswordUser(string userId, string lastPassword, string newPassword,
        string newPasswordConfirmation);
}