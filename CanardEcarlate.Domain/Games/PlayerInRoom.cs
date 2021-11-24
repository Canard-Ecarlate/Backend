using System.Collections.Generic;
using CanardEcarlate.Domain.Cards;
using CanardEcarlate.Domain.Roles;

namespace CanardEcarlate.Domain.Games
{
    public class PlayerInRoom
    {
        public string Name { get; set; }
        public IRole Role { get; set; }
        public List<ICard> CardsInHand { get; set; }
    }
}