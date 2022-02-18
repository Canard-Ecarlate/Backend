using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Cards
{
    public class Card : ICard
    {
        private Card()
        {
        }

        public string Name => "Default";

        /*
         * Update CurrentPlayerId and IsCardsDrawable of players
         */
        public void DrawAction(Player playerWhoDraw, Player playerWhereCardIsDrawing, Game game, HashSet<Player> players)
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