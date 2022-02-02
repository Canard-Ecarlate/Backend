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
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string HostId { get; set; }
        public string HostName { get; set; }
        public string ContainerId { get; set; }
        public bool IsPlaying { get; set; } = false;
        public HashSet<string> PlayersId { get; set; }
        public RoomConfiguration RoomConfiguration { get; set; }

        public Room(string name, string hostId, string hostName, bool isPrivate, int nbPlayers)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Name = name;
            Code = "Code to generate";
            HostId = hostId;
            HostName = hostName;
            ContainerId = "To do";
            RoomConfiguration = new RoomConfiguration(isPrivate, nbPlayers);
            PlayersId = new HashSet<string> {hostId};
        }
    }
}