using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Cards
{
    public abstract class Card : ICard
    {
        public string Name => "Default";

        /*
         * Update CurrentPlayerId and IsCardsDrawable of players
         */
        public void DrawAction(Player playerWhoDraw, Player playerWhereCardIsDrawing, Game game, HashSet<Player> players)
        {
            game.CurrentPlayerId = playerWhereCardIsDrawing.Id;
            playerWhoDraw.IsCardsDrawable = true;
            playerWhoDraw.IsCardsDrawable = false;
        }
    }
}