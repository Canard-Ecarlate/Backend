using DuckCity.Api.DTO.User;
using DuckCity.Application.UserService;
using DuckCity.Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public ActionResult<string> DeleteUser()
    {
        string userId = UserUtils.GetPayloadFromToken(HttpContext, "userId");
        _userService.DeleteAccountUser(userId);
        return new OkObjectResult("The account is deleted");
    }
    
    [HttpPost]
    public ActionResult<string> ChangePassword(UserChangePassword user)
    {
        string userId = UserUtils.GetPayloadFromToken(HttpContext, "userId");
        _userService.ChangePasswordUser(userId,user.ActualPassword,user.NewPassword,user.PasswordConfirmation);
        return new OkObjectResult("The password has been changed");
    }
    
}