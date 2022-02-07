using DuckCity.Domain.Cards;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Games
{
    public class Game
    {
        public string RoomId { get; init; }
        public int NbRedPlayers { get; set; }
        public HashSet<Player> Players { get; }
        public string? CurrentPlayerId { get; set; }
        public string? PreviousPlayerId { get; set; }
        public ICard? PreviousDrawnCard { get; set; }
        public int NbRound { get; set; }
        public int NbGreenDrawn { get; set; }
        public int NbDrawnDuringRound { get; set; }

        public Game(string roomId, string connectionId, string hostId, string hostName)
        {
            RoomId = roomId;
            Players = new HashSet<Player> {new(connectionId, hostId, hostName)};
        }
    }
}
