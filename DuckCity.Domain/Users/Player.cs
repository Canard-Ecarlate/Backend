using DuckCity.Domain.Cards;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Roles;

namespace DuckCity.Domain.Users;

public class Player
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? ConnectionId { get; set; }
    public bool Ready { get; set; }
    public IRole? Role { get; set; }
    public List<ICard> CardsInHand { get; set; } = new();
    public bool IsCardsDrawable { get; set; } = true;

    public Player(string connectionId, string userId, string userName)
    {
        ConnectionId = connectionId;
        Id = userId;
        Name = userName;
    }

    public Type DrawCard()
    {
        Random random = new Random();
        int nbCardsInHand = CardsInHand.Count;
        if (nbCardsInHand == 0)
        {
            throw new CardsInHandEmptyException();
        }
        Type typeDrawnCard = CardsInHand.ElementAt(random.Next(nbCardsInHand)).GetType();
        CardsInHand.RemoveAt(nbCardsInHand);
        return typeDrawnCard;
    }
}