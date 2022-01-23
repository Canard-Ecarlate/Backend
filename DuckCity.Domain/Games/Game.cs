using DuckCity.Domain.Cards;
using DuckCity.Domain.Rooms;

namespace DuckCity.Domain.Games
{
    public class Game
    {
        public Room? Room { get; init; }
        public int NbRedPlayers { get; set; }
        public HashSet<PlayerInGame> Players { get; } = new();
        public string? CurrentPlayerId { get; set; }
        public string? PreviousPlayerId { get; set; }
        public ICard? PreviousDrawnCard { get; set; }
        public int NbRound { get; set; }
        public int NbGreenDrawn { get; set; }
        public int NbDrawnDuringRound { get; set; }
    }
}
