using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Cards
{
    public class BombCard : Card
    {
        override
        public string Name => "Bomb";

        // Reds Win
        override
        public void DrawAction(Player playerWhoDraw, Player playerWhereCardIsDrawing, Game game, HashSet<Player> players)
        {
            base.DrawAction(playerWhoDraw, playerWhereCardIsDrawing, game, players);
            game.EndGame();
        }
    }
}