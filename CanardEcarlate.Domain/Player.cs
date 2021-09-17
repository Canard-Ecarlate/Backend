using CanardEcarlate.Domain.Cards;
using CanardEcarlate.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain
{
    public class Player
    {
        public string Name { get; set; }
        public IRole Role { get; set; }
        public List<ICard> Cards { get; set; }
    }
}
