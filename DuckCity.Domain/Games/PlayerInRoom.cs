using DuckCity.Domain.Cards;
using DuckCity.Domain.Roles;

namespace DuckCity.Domain.Games
{
    public class PlayerInRoom
    {
        public string? Id { get; init; }
        public string? Name { get; set; }
        public IRole? Role { get; set; }
        public List<ICard>? CardsInHand { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlayerInRoom playerInRoom && Id == playerInRoom.Id;
        }

        public override int GetHashCode()
        {
            return new {Id}.GetHashCode();
        }
    }
}