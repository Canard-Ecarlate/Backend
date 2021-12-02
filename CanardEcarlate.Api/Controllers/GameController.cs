using System;
using CanardEcarlate.Api.Models;
using CanardEcarlate.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace CanardEcarlate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<CanardEcarlateHub> _ceHub;

        public GameController(IHubContext<CanardEcarlateHub> ceHub, IConfiguration configuration)
        {
            _ceHub = ceHub;
            Configuration = configuration;
        }
        // Exemple de récupération des infos dans appSettings
        public IConfiguration Configuration { get; }

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