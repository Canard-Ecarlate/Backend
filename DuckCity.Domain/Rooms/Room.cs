using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckCity.Domain.Rooms
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Id { get; set; }

        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? HostId { get; set; }
        public string? HostName { get; set; }
        public string? ContainerId { get; set; }
        public bool IsPlaying { get; set; }
        public RoomConfiguration? RoomConfiguration { get; set; }
        public HashSet<PlayerInRoom>? Players { get; init; }
    }
}