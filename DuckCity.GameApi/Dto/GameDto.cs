using DuckCity.Domain.Games;

namespace DuckCity.GameApi.Dto
{
    public class GameDto 
    {
        public PlayerMeDto? playerMe { get; set; }
        public Game? game { get; set; }
        public HashSet<string>? playerDrawable { get; set; }
        public HashSet<OtherPlayerDto>? OtherPlayers { get; set; }
    }
}