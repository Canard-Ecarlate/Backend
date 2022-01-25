using DuckCity.Application.Services;
using DuckCity.GameApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.GameApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RoomController
{
    private readonly RoomService _roomService;

    public RoomController(RoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost]
    public ActionResult<bool> LeaveRoom(UserAndRoomDto userAndRoomDto)
    {
        bool left = _roomService.LeaveRoom(userAndRoomDto.RoomId, userAndRoomDto.UserId);
        return new OkObjectResult(left);
    }
}
