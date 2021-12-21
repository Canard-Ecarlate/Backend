using DuckCity.Domain.Cards;
using DuckCity.Domain.Roles;

namespace DuckCity.Domain.Games
{
    public class DataInGame
    {
        public PlayerInRoom? CurrentPlayerInGame { get; set; }
        public PlayerInRoom? PreviousPlayerInGame { get; set; }
        public ICard? DrawnCard { get; set; }
        public int NbRound { get; set; }
        public int NbGreenDrawn { get; set; }
        public int NbDrawnDuringRound { get; set; }
        public IRole? WinnerRole { get; set; } = null;
    }
}
