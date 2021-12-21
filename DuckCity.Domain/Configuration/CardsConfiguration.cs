using DuckCity.Domain.Cards;

namespace DuckCity.Domain.Configuration
{
    public class CardsConfiguration
    {
        public int NbPlayers { get; set; }
        public Dictionary<ICard, int>? NbEachCard { get; set; }
    }
}
