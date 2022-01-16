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
            Room roomJoined = _roomService.JoinRooms(userJoinRoom.RoomId, userJoinRoom.UserId);
            return new OkObjectResult(roomJoined);
        }
        
        [HttpPost]
        public ActionResult<string> ReadyToPlay(string userName)
        {
            return new OkObjectResult(userName + " ready to play");
        }
        
        [HttpPost]
        public ActionResult<string> LeaveRoom()
        {
            // Si 0 joueurs, destroy
            return new OkObjectResult("destroy room");
        }
        
        [HttpPost]
        public ActionResult<string> StartGame()
        {
            // Création objet Game en mémoire sur lequel tous les joueurs vont jouer
            // global stats nb all games +1
            return new OkObjectResult("start game");
        }

        [HttpPost]
        public ActionResult<string> RestartGame()
        {
            // global stats nb replays +1
            return new OkObjectResult("draw card");
        }

        [HttpPost]
        public ActionResult<string> StopGame()
        {
            // nb uit mid game +1
            return new OkObjectResult("stop game");
        }
    }
}