using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CanardEcarlate.Domain
{
    public class CardsConfiguration
    {
        public int NbPlayers { get; set; }
        public Dictionary<ICard, int> NbEachCard { get; set; }
    }
}
