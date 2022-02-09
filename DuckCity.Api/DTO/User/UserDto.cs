using System.ComponentModel.DataAnnotations;

namespace DuckCity.Api.DTO.User;

public class UserDto
{
    [Required]
    public string UserId { get; init; } = "";
    [Required]
    public string UserMail { get; init; } = "";
    [Required]
    public string UserName { get; init; } = "";
}