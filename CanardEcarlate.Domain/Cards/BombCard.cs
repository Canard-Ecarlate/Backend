using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain.Cards
{
    public class BombCard : ICard
    {
        public string Name => "Bomb";

        public void DrawAction()
        {
            // EXPLOSION
        }
    }
}
