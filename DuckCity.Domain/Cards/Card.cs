using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Cards
{
    public class Card : ICard
    {
        public virtual string Name => "Default";

        /*
         * Update CurrentPlayerId and IsCardsDrawable of players
         */
        public virtual void DrawAction(Player playerWhoDraw, Player playerWhereCardIsDrawing, Game game, HashSet<Player> players)
        {
            game.CurrentPlayerId = playerWhereCardIsDrawing.Id;
            if (playerWhoDraw.CardsInHand.Count != 0)
            {
                playerWhoDraw.IsCardsDrawable = true; 
            }
            playerWhereCardIsDrawing.IsCardsDrawable = false;
        }
    }
}