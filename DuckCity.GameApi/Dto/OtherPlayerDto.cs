namespace DuckCity.GameApi.Dto
{
    public class OtherPlayerDto
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int NbCardsInHand { get; set; }

        public OtherPlayerDto(string playerId, string playerName, int nbCardsInHand)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            NbCardsInHand = nbCardsInHand;
        }
    }
}