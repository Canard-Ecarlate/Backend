using DuckCity.Application.Services;
using DuckCity.GameApi.Models;
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
    public ActionResult<bool> LeaveRoom(UserAndRoom userAndRoom)
    {
        bool left = _roomService.LeaveRoom(userAndRoom.RoomId, userAndRoom.UserId);
        return new OkObjectResult(left);
    }
}
