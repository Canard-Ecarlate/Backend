using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.Repositories;

public interface IPlayerRepository
{
    void ConnectPlayer(string connectionId, string userId, string userName, string roomId);
    void ReconnectPlayer(Player player, string connectionId);
    void RemovePlayer(Player player);
    Player FindPlayerByConnectionId(string connectionId);
    Player? FindPlayerByUserId(string userId);
    IEnumerable<Player> FindPlayersInRoom(string roomId);
    string ReadyToPlay(string connectionId);
    string? DisconnectPlayer(string connectionId);
}