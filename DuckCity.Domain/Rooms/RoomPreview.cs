using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckCity.Domain.Rooms;

public class RoomPreview
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string HostId { get; set; }
    public string HostName { get; set; }
    public string ContainerId { get; set; }
    public bool IsPlaying { get; set; }
    public HashSet<string> PlayersId { get; set; }
    public RoomConfiguration RoomConfiguration { get; set; }

    public RoomPreview(Room room)
    {
        Id = room.Id;
        Name = room.Name;
        Code = room.Code;
        HostId = room.HostId;
        HostName = room.HostName;
        ContainerId = room.ContainerId;
        RoomConfiguration = room.RoomConfiguration;
        PlayersId = room.Players.Select(p => p.Id).ToHashSet();
    }
}