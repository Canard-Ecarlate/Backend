using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Domain.Rooms;

public class Room
{
    public string RoomId { get; set; }
    public HashSet<Player> Players { get; }
    public Game? Game { get; set; }

    public Room(string roomId, string connectionId, string hostId, string hostName)
    {
        RoomId = roomId;
        Players = new HashSet<Player> {new(connectionId, hostId, hostName)};
    }
}