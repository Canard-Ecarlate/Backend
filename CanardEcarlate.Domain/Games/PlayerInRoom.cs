using System;
using System.Collections.Generic;
using CanardEcarlate.Domain.Cards;
using CanardEcarlate.Domain.Roles;

namespace CanardEcarlate.Domain.Games
{
    public class PlayerInRoom
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IRole Role { get; set; }
        public List<ICard> CardsInHand { get; set; }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                PlayerInRoom p = (PlayerInRoom)obj;
                return (Id == p.Id);
            }
        }
    }
}