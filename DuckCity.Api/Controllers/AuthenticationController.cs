using DuckCity.Api.DTO.Authentication;
using DuckCity.Application.AuthenticationService;
using DuckCity.Application.RoomPreviewService;
using DuckCity.Application.Utils;
using DuckCity.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IRoomPreviewService _roomPreviewService;

    public AuthenticationController(IAuthenticationService authenticationService,
        IRoomPreviewService roomPreviewService)
    {
        _authenticationService = authenticationService;
        _roomPreviewService = roomPreviewService;
    }

    [HttpPost]
    [Route("")]
    public ActionResult<TokenAndCurrentContainerIdDto> Login(IdentifierDto identifierDto)
    {
        User user = _authenticationService.Login(identifierDto.Name, identifierDto.Password);

        TokenAndCurrentContainerIdDto tokenAndCurrentContainerIdDto =
            new(user.Id, user.Name, _authenticationService.GenerateJsonWebToken(user),
                _roomPreviewService.FindByUserId(user.Id)?.ContainerId);
        return new OkObjectResult(tokenAndCurrentContainerIdDto);
    }

    [HttpPost]
    [Route("")]
    public ActionResult<TokenAndCurrentContainerIdDto> SignUp(RegisterDto registerDto)
    {
        _authenticationService.SignUp(registerDto.Name, registerDto.Email, registerDto.Password,
            registerDto.PasswordConfirmation);
        return Login(new IdentifierDto {Name = registerDto.Name, Password = registerDto.Password});
    }

    [HttpPost]
    [Route("")]
    [Authorize]
    public ActionResult<TokenAndCurrentContainerIdDto> CheckToken()
    {
        string userId = UserUtils.GetPayloadFromToken(HttpContext, "userId");
        string userName = UserUtils.GetPayloadFromToken(HttpContext, "userName");
        string? containerId = _roomPreviewService.FindByUserId(userId)?.ContainerId;
        TokenAndCurrentContainerIdDto dto = new(userId, userName, "", containerId);
        return new OkObjectResult(dto);
    }
}