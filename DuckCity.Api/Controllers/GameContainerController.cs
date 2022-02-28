using DuckCity.Api.DTO.Room;
using DuckCity.Application.ContainerGameApiService;
using DuckCity.Application.Utils;
using DuckCity.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class GameContainerController : ControllerBase
{
    private readonly IGameContainerService _gameContainerService;

    public GameContainerController(IGameContainerService gameContainerService)
    {
        _gameContainerService = gameContainerService;
    }

    [HttpPost]
    public ActionResult<GameContainer> FindContainerIdForCreateRoom(RoomCreationDto dto)
    {
        string userId = UserUtils.GetPayloadFromToken(HttpContext, "userId");
        GameContainer access = _gameContainerService.ContainerAccessToCreateRoom(dto.RoomName, userId);
        return new OkObjectResult(access);
    }

    [HttpPost]
    public ActionResult<string> FindContainerIdForJoinRoom(UserAndRoomDto dto)
    {
        string userId = UserUtils.GetPayloadFromToken(HttpContext, "userId");
        GameContainer access = _gameContainerService.ContainerAccessToJoinRoom(dto.RoomCode, userId);
        return new OkObjectResult(access);
    }
}