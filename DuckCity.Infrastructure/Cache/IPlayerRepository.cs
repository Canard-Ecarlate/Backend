using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.Cache;

public interface IPlayerRepository
{
    void AddOrReconnectPlayer(string connectionId, string userId, string userName, string roomId);
    void RemovePlayer(Player player);
    Player FindPlayerByConnectionId(string connectionId);
    Player FindPlayerByUserId(string userId);
    IEnumerable<Player> FindPlayersInRoom(string roomId);
    string ReadyToPlay(string connectionId);
    string? DisconnectToRoom(string connectionId);
}