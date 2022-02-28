using DuckCity.Domain.Games;
using DuckCity.Domain.Users;
using MongoDB.Bson;

namespace DuckCity.Domain.Rooms;

public class Room
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string HostId { get; set; }
    public string HostName { get; set; }
    public string ContainerId { get; set; }
    public RoomConfiguration RoomConfiguration { get; set; }
    public HashSet<Player> Players { get; }
    public Game? Game { get; set; }

    public Room(string name, string hostId, string hostName, string containerId, bool isPrivate,
        int nbPlayers, string connectionId,string code)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Name = name;
        HostId = hostId;
        HostName = hostName;
        ContainerId = containerId;
        RoomConfiguration = new RoomConfiguration(isPrivate, nbPlayers);
        Players = new HashSet<Player> {new(connectionId, hostId, hostName)};
        Code = code;
    }
}