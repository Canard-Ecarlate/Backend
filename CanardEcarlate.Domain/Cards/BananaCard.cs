using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain.Cards
{
    class BananaCard : ICard
    {
        public string Name => "Banana";

        public void DrawAction()
        {
            // Skip the current player
        }
    }
}
