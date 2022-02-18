using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Cards
{
    class BananaCard : ICard
    {
        public string Name => "Banana";

        public void DrawAction()
        {
            // Skip the current player
        }

        public void DrawAction(Player playerWhoDraw, Player playerWhereCardIsDrawing, Game game, HashSet<Player> players)
        {
            throw new NotImplementedException();
        }
    }
}
