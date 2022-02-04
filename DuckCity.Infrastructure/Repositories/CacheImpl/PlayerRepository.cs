using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.Repositories.CacheImpl;

public class PlayerRepository : IPlayerRepository
{
    private static readonly HashSet<Player> Players = new();

    public void ConnectPlayer(string connectionId, string userId, string userName, string roomId) =>
        Players.Add(new Player(connectionId, userId, userName, roomId));

    public void ReconnectPlayer(Player player, string connectionId)
    {
        player.ConnectionId = connectionId;
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
    }

    public Player FindPlayerByConnectionId(string connectionId)
    {
        return Players.Single(u => u.ConnectionId == connectionId);
    }

    public Player? FindPlayerByUserId(string userId)
    {
        return Players.SingleOrDefault(u => u.Id == userId);
    }

    public IEnumerable<Player> FindPlayersInRoom(string roomId)
    {
        return Players.Where(u => u.RoomId == roomId);
    }

    public string ReadyToPlay(string connectionId)
    {
        Player player = FindPlayerByConnectionId(connectionId);
        player.Ready = !player.Ready;
        return player.RoomId;
    }

    public string? DisconnectPlayer(string connectionId)
    {
        Player? player = Players.SingleOrDefault(u => u.ConnectionId == connectionId);
        if (player != null)
        {
            player.ConnectionId = null;
            return player.RoomId;
        }

        return null;
    }
}