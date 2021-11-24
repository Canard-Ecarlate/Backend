using CanardEcarlate.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CanardEcarlate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        
        private readonly IHubContext<CanardEcarlateHub> _ceHub;

        public GameController(IHubContext<CanardEcarlateHub> ceHub)
        {
            _ceHub = ceHub;
        }

        
        [HttpPost]
        public ActionResult<string> DrawCard(string userName)
        {
            // Exemple d'utilisation de signalR
            _ceHub.Clients.All.SendAsync("AfterDrawCard", "card drawn");
            
            // If win => global stat nbWonAs....ByNbPlayers +1
            return new OkObjectResult("draw card in " + userName + " 's hand");
        }
    }
}