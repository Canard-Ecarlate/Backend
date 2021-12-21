using DuckCity.Api.Models.Room;
using DuckCity.Application;
using DuckCity.Domain.Games;
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

        [HttpPost]
        public ActionResult<Room> CreateRoom(RoomCreation room)
        {
            Room roomCreated = _roomService.AddRooms(room.RoomName, room.HostId, room.GameConfiguration, room.IsPrivate);
            return JoinRoom(new UserJoinRoom { RoomId = roomCreated.Id, UserId = room.HostId });
        }
        
        [HttpPost]
        public ActionResult<Room> JoinRoom(UserJoinRoom userJoinRoom)
        {
            // Récuperer le containerId
            // Appel rest sur localhost en test, sur le containerID en prod
            Room roomJoined = _roomService.JoinRooms(userJoinRoom.RoomId, userJoinRoom.UserId);
            
            return new OkObjectResult(roomJoined);
        }
    }
}