using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Cards
{
    public class BombCard : Card
    {
        public new string Name => "Bomb";

        // Reds Win
        public new void DrawAction(Player playerWhoDraw, Player playerWhereCardIsDrawing, Game game, HashSet<Player> players)
        {
            base.DrawAction(playerWhoDraw, playerWhereCardIsDrawing, game, players);
            game.EndGame();
        }
    }
}