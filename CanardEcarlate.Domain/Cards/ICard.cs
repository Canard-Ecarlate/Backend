using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain.Cards
{
    public interface ICard
    {
        string Name { get; }

        void DrawAction();
    }
}
