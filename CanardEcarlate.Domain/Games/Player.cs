using System.Collections.Generic;

namespace CanardEcarlate.Domain
{
    public class Player
    {
        public string Name { get; set; }
        public IRole Role { get; set; }
        public List<ICard> Cards { get; set; }
    }
}