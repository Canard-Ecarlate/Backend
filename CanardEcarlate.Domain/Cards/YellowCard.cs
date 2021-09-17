using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain.Cards
{
    class YellowCard : ICard
    {
        public string Name => "Yellow";

        public void DrawAction()
        {
            throw new NotImplementedException();
        }
    }
}
