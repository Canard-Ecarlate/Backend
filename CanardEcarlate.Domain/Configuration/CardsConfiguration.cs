using System.Collections.Generic;
using CanardEcarlate.Domain.Cards;

namespace CanardEcarlate.Domain.Configuration
{
    public class CardsConfiguration
    {
        public int NbPlayers { get; set; }
        public Dictionary<ICard, int> NbEachCard { get; set; }
    }
}
