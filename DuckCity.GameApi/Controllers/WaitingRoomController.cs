using DuckCity.Application.Services.Interfaces;
using DuckCity.GameApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DuckCity.GameApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class WaitingRoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public WaitingRoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost]
    public ActionResult<bool> LeaveRoom(UserIdAndRoomIdDto userIdAndRoomIdDto)
    {
        bool left = _roomService.LeaveRoom(userIdAndRoomIdDto.RoomId, userIdAndRoomIdDto.UserId);
        return new OkObjectResult(left);
    }
}
