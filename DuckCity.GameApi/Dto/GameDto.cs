using DuckCity.Domain.Games;

namespace DuckCity.GameApi.Dto
{
    public class GameDto 
    {
        public PlayerMeDto PlayerMe { get; set; }
        public Game Game { get; set; }
        public HashSet<string> PlayerDrawable { get; set; }
        public HashSet<OtherPlayerDto> OtherPlayers { get; set; }

        public GameDto(PlayerMeDto playerMe, Game game, HashSet<string> playersWithCardsDrawable, HashSet<OtherPlayerDto> otherPlayers)
        {
            PlayerMe = playerMe;
            Game = game;
            PlayerDrawable = playersWithCardsDrawable;
            OtherPlayers = otherPlayers;
        }
    }
}