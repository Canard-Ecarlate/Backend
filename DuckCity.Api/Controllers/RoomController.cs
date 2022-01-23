using DuckCity.Api.Models.Room;
using DuckCity.Application.Services;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Room>> FindAllRooms() => new OkObjectResult(_roomService.FindAllRooms());

        [HttpPost]
        public ActionResult<Room> CreateRoom(RoomCreation room)
        {
            Room roomCreated = _roomService.AddRooms(room.Name, room.HostId, room.HostName, room.IsPrivate, room.NbPlayers);
            if (roomCreated.Id == null)
            {
                throw new RoomIdNoExistException();
            }
            ActionResult<Room> newRoom = JoinRoom(new UserAndRoom {UserId = room.HostId, UserName = room.HostName, RoomId = roomCreated.Id});
            return newRoom;
        }
        
        [HttpPost]
        public ActionResult<Room> JoinRoom(UserAndRoom userAndRoom)
        {
            Room roomJoined = _roomService.JoinRoom(userAndRoom.RoomId, userAndRoom.UserId, userAndRoom.UserName);
            return new OkObjectResult(roomJoined);
        }
    }
}