using CanardEcarlate.Api.Models;
using Microsoft.AspNetCore.Mvc;
using CanardEcarlate.Application;
using Microsoft.AspNetCore.SignalR;
using CanardEcarlate.Domain.Games;

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
        public ActionResult<string> CreateRoom(RoomCreation room)
        {
            Room roomCreated = _roomService.AddRooms(room.RoomName, room.HostId, room.GameConfiguration, room.IsPrivate);
            JoinRoom(new UserJoinRoom { RoomId = roomCreated.Id,UserName = room.RoomName});
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