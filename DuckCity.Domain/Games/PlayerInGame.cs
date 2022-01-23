using DuckCity.Domain.Cards;
using DuckCity.Domain.Roles;

namespace DuckCity.Domain.Games
{
    public class PlayerInGame
    {
        public string? Id { get; init; }
        public string? Name { get; init; }
        public IRole Role { get; set; } = new BlueRole();
        public List<ICard> CardsInHand { get; set; } = new();
    }
}