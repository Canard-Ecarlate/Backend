using CanardEcarlate.Domain.Cards;
using CanardEcarlate.Domain.Roles;

namespace CanardEcarlate.Domain.Games
{
    public class DataInGame
    {
        public PlayerInGame CurrentPlayerInGame { get; set; }
        public PlayerInGame PreviousPlayerInGame { get; set; }
        public ICard DrawnCard { get; set; }
        public int NbRound { get; set; }
        public int NbGreenDrawn { get; set; }
        public int NbDrawnDuringRound { get; set; }
        public IRole WinnerRole { get; set; } = null;
    }
}
