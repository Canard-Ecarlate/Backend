using Microsoft.AspNetCore.Mvc;

namespace DuckCity.GameApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class GameController
{
    [HttpPost]
    public ActionResult<string> StartGame()
    {
        // Création objet Game en mémoire sur lequel tous les joueurs vont jouer
        // global stats nb all games +1
        return new OkObjectResult("start game");
    }

    [HttpPost]
    public ActionResult<string> DrawCard(string userName)
    {
        return new OkObjectResult("draw card in " + userName + " 's hand");
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