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
    public ActionResult<GameContainer> ContainerAccessToCreateRoom(RoomCreationDto room)
    {
        GameContainer access = _gameContainerService.ContainerAccessToCreateRoom(room.Name, room.HostId);
        return new OkObjectResult(access);
    }

    [HttpPost]
    public ActionResult<string> ContainerAccessToJoinRoom(UserAndRoomDto userAndRoomDto)
    {
        GameContainer access =
            _gameContainerService.ContainerAccessToJoinRoom(userAndRoomDto.RoomId, userAndRoomDto.UserId);
        return new OkObjectResult(access);
    }
}