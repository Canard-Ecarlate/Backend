using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        public GameController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // Exemple de récupération des infos dans appSettings
        public IConfiguration Configuration { get; }

        [HttpPost]
        public ActionResult<string> DrawCard(string userName)
        {
            return new OkObjectResult("draw card in " + userName + " 's hand");
        }
    }
}