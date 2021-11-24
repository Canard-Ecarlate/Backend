using CanardEcarlate.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CanardEcarlate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IHubContext<CanardEcarlateHub> _ceHub;

        public RoomController(IHubContext<CanardEcarlateHub> ceHub)
        {
            _ceHub = ceHub;
        }

        [HttpPost]
        public ActionResult<string> CreateRoom()
        {
            // Exemple d'utilisation de signalR
            _ceHub.Clients.All.SendAsync("AfterCreateRoom", "Room created");
            return new OkObjectResult("create room");
        }
        
        [HttpPost]
        public ActionResult<string> JoinRoom()
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