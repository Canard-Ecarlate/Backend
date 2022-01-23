using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckCity.Domain.Statistics
{
    public class UserStatistics
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Id { get; set; }
        public int UserName { get; set; }
        public int NbGamesPlayed { get; set; }
        public int NbWonAsBlue { get; set; }
        public int NbLostAsBlue { get; set; }
        public int NbWonAsRed { get; set; }
        public int NbLostAsRed { get; set; }
        public int Streak { get; set; }
        public int MaxWinStreak { get; set; }
        public int MaxLossStreak { get; set; }
        public List<CardsConfigurationStatistics>? CardsConfigurationStatistics { get; set; }
    }
}
