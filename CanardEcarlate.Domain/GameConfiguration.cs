using CanardEcarlate.Domain.BDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain
{
    public class GameConfiguration
    {
        public bool IsPrivate { get; set; }
        public string Code { get; set; }
        public CardsConfiguration CardsConfiguration { get; set; }
        public bool IsPlaying { get; set; }
    }
}
