using DuckCity.Domain.Cards;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Games
{
    public class Game
    {
        public int NbRedPlayers { get; set; }
        public string? CurrentPlayerId { get; set; }
        public string? PreviousPlayerId { get; set; }
        public ICard? PreviousDrawnCard { get; set; }
        public int NbRound { get; set; }
        public int NbGreenDrawn { get; set; }
        public int NbDrawnDuringRound { get; set; }
    }
}
