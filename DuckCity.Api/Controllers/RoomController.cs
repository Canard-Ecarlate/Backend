using DuckCity.Api.DTO.Room;
using DuckCity.Application.Services;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Room>> FindAllRooms() => new OkObjectResult(_roomService.FindAllRooms());

    [HttpPost]
    public ActionResult<Room> CreateRoom(RoomCreationDto room)
    {
        Room roomCreated = _roomService.AddRooms(room.Name, room.HostId, room.HostName, room.IsPrivate, room.NbPlayers);
        if (roomCreated.Id == null)
        {
            Room roomCreated = _roomService.AddRooms(room.Name, room.HostId, room.HostName, room.IsPrivate, room.NbPlayers);
            return roomCreated;
        }
        ActionResult<Room> newRoom = JoinRoom(new UserAndRoomDto {UserId = room.HostId, UserName = room.HostName, RoomId = roomCreated.Id});
        return newRoom;
    }
        
    [HttpPost]
    public ActionResult<Room> JoinRoom(UserAndRoomDto userAndRoomDto)
    {
        Room roomJoined = _roomService.JoinRoom(userAndRoomDto.RoomId, userAndRoomDto.UserId, userAndRoomDto.UserName);
        return new OkObjectResult(roomJoined);
    }
}