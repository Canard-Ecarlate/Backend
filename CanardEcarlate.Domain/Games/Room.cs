using System.Collections.Generic;
using System.Text.Json.Serialization;
using CanardEcarlate.Domain.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CanardEcarlate.Domain.Games
{
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string HostName { get; set; }
        public GameConfiguration GameConfiguration { get; set; }
        public List<PlayerInRoom> Players { get; set; }
        public DataInGame DataInGame { get; set; }
        public bool IsPrivate { get; set; }

    }
}
