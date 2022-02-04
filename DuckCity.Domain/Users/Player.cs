using DuckCity.Domain.Cards;
using DuckCity.Domain.Roles;

namespace DuckCity.Domain.Users;

public class Player : User
{
    public string RoomId { get; set; }
    public string? ConnectionId { get; set; }
    public bool Ready { get; set; }
    public IRole Role { get; set; } = new BlueRole();
    public List<ICard> CardsInHand { get; set; } = new();
    
    public Player(string connectionId, string userId, string userName, string roomId)
    {
        ConnectionId = connectionId;
        Id = userId;
        Name = userName;
        RoomId = roomId;
    }
}