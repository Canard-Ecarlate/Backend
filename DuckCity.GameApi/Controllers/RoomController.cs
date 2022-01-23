using DuckCity.Application.Services;
using DuckCity.Domain.Rooms;
using DuckCity.GameApi.Hub;
using DuckCity.GameApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RoomController
{
    private readonly RoomService _roomService;
    private readonly IHubContext<DuckCityHub> _duckCityHub;

    public RoomController(RoomService roomService, IHubContext<DuckCityHub> duckCityHub)
    {
        _roomService = roomService;
        _duckCityHub = duckCityHub;
    }

    [HttpPost]
    public ActionResult<Room> JoinRoom(UserAndRoom userAndRoom)
    {
        Room roomJoined = _roomService.JoinRooms(userAndRoom.RoomId, userAndRoom.UserId, userAndRoom.UserName);
        return new OkObjectResult(roomJoined);
    }

    [HttpPost]
    public ActionResult<string> LeaveRoom()
    {
        // Si 0 joueurs, destroy
        return new OkObjectResult("destroy room");
    }

}