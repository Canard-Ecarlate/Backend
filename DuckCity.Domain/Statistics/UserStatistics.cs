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
        [BsonElement("username")]
        public int UserName { get; set; }
        [BsonElement("nb_played_game")]
        public int NbGamesPlayed { get; set; }
        [BsonElement("nb_won_as_blue")]
        public int NbWonAsBlue { get; set; }
        [BsonElement("nb_lost_as_blue")]
        public int NbLostAsBlue { get; set; }
        [BsonElement("nb_won_as_red")]
        public int NbWonAsRed { get; set; }
        [BsonElement("nb_lost_as_red")]
        public int NbLostAsRed { get; set; }
        [BsonElement("streak")]
        public int Streak { get; set; }
        [BsonElement("max_win_streak")]
        public int MaxWinStreak { get; set; }
        [BsonElement("max_loss_streak")]
        public int MaxLossStreak { get; set; }
        [BsonElement("cards_configuration_statistics")]
        public List<CardsConfigurationStatistics>? CardsConfigurationStatistics { get; set; }
    }
}
