using Microsoft.AspNetCore.Mvc;

namespace CanardEcarlate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> DrawCard(string userName)
        {
            // If win => global stat nbWonAs....ByNbPlayers +1
            return new OkObjectResult("draw card in " + userName + " 's hand");
        }
    }
}