using DuckCity.Domain.Cards;
using DuckCity.Domain.Roles;

namespace DuckCity.Domain.Rooms
{
    public class RoomConfiguration
    {
        public bool IsPrivate { get; set; }
        public int NbPlayers { get; set; }
        public List<NbEachCard> Cards { get; set; }
        public List<NbEachRole> Roles { get; set; }
        private const int NumberOfCardsFirstRound = 5;

        public RoomConfiguration(bool isPrivate, int nbPlayers)
        {
            IsPrivate = isPrivate;
            NbPlayers = nbPlayers;
            Cards = new List<NbEachCard>
            {
                new("Bomb",1),
                new("Green",NbPlayers),
                new("Yellow", (NbPlayers * NumberOfCardsFirstRound) - NbPlayers + 1)
            };
            int redPlayerNumber = NbPlayers / 2;
            if (NbPlayers % 2 == 0)
            {
                Random random = new Random();
                int rnd = random.Next(4);
                if(rnd % 2 == 0)
                {
                    redPlayerNumber--;
                }
            }
            Roles = new List<NbEachRole>
            {
                new("Red", redPlayerNumber),
                new("Blue", NbPlayers - redPlayerNumber)
            };
        }
    }
}