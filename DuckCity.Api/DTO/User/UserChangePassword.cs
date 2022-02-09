using System.ComponentModel.DataAnnotations;

namespace DuckCity.Api.DTO.User;

public class UserChangePassword
{
    [Required]
    public string UserId { get; init; } = "";
    [Required]
    public string ActualPassword { get; init; } = "";
    [Required]
    public string NewPassword { get; init; } = "";
    [Required]
    public string PasswordConfirmation { get; init; } = "";
}