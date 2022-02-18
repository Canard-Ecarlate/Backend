using DuckCity.Api.DTO.Room;
using DuckCity.Application.ContainerGameApiService;
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
    public ActionResult<GameContainer> ContainerAccessToCreateRoom(RoomCreationDto dto)
    {
        GameContainer access = _gameContainerService.ContainerAccessToCreateRoom(dto.Name, dto.HostId);
        return new OkObjectResult(access);
    }

    [HttpPost]
    public ActionResult<string> ContainerAccessToJoinRoom(UserAndRoomDto dto)
    {
        GameContainer access = _gameContainerService.ContainerAccessToJoinRoom(dto.RoomCode, dto.UserId);
        return new OkObjectResult(access);
    }
}