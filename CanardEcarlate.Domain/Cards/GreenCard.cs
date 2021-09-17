using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain.Cards
{
    class GreenCard : ICard
    {
        public string Name => "Green";

        public void DrawAction()
        {
            // Increment Green cards discovered
        }
    }
}
