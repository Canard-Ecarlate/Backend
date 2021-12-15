using CanardEcarlate.Api.Models;
using Microsoft.AspNetCore.Mvc;
using CanardEcarlate.Application;

namespace CanardEcarlate.Api.Controllers
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
        public ActionResult<string> CreatePublicRoom(RoomCreation room)
        {
            _roomService.AddPublicRooms(room.RoomName, room.HostName, room.GameConfiguration);
            JoinRoom(new UserJoinRoom { RoomName = room.RoomName,UserName = room.RoomName});
            return new OkObjectResult("Room created");
        }

        [HttpPost]
        public ActionResult<string> CreatePrivateRoom(RoomCreation room)
        {
            _roomService.AddPrivateRooms(room.RoomName, room.HostName, room.GameConfiguration);
            JoinRoom(new UserJoinRoom { RoomName = room.RoomName, UserName = room.RoomName });
            return new OkObjectResult("Room created");
        }

        [HttpPost]
        public ActionResult<string> JoinRoom(UserJoinRoom user)
        {
            return new OkObjectResult("join room");
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