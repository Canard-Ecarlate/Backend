using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CanardEcarlate.Domain.Statistics
{
    public class UserStatistics
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }
        [BsonElement("username")]
        public int UserName { get; set; }
        [BsonElement("nbplayedgame")]
        public int NbGamesPlayed { get; set; }
        [BsonElement("nbwonasciat")]
        public int NbWonAsCIAT { get; set; }
        [BsonElement("nblostasciat")]
        public int NbLostAsCIAT { get; set; }
        [BsonElement("nbwonasce")]
        public int NbWonAsCE { get; set; }
        [BsonElement("nblostasce")]
        public int NbLostAsCe { get; set; }
        [BsonElement("streak")]
        public int Streak { get; set; }
        [BsonElement("maxwinstreak")]
        public int MaxWinStreak { get; set; }
        [BsonElement("maxlossstreak")]
        public int MaxLossStreak { get; set; }
        [BsonElement("cardsconfigurationstatistics")]
        public List<CardsConfigurationStatistics> CardsConfigurationStatistics { get; set; }
    }
}
