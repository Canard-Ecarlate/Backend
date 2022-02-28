using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Cards
{
    class GreenCard : Card
    {
        override
        public string Name => "Green";

        /*
         * Update the number of drawn green during the game
         */
        override
        public void DrawAction(Player playerWhoDraw, Player playerWhereCardIsDrawing, Game game, HashSet<Player> players)
        {
            base.DrawAction(playerWhoDraw, playerWhereCardIsDrawing, game, players);
            game.NbGreenDrawn++;
            if (game.NbGreenDrawn == game.NbCardsToDrawByRound)
            {
                game.EndGame();
            }
        }
    }
}