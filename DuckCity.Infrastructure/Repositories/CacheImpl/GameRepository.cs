using DuckCity.Domain.Games;
using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.Repositories.CacheImpl;

public class GameRepository : IGameRepository
{
    private static readonly HashSet<Game> Games = new();
    public Game? FindGameByRoomId(string roomId)
    {
        return Games.SingleOrDefault(g => g.RoomId == roomId);
    }
    
    private static Game? FindGameByConnectionId(string connectionId)
    {
        return Games.SingleOrDefault(g => g.Players.SingleOrDefault(p => p.ConnectionId == connectionId) != null);
    }

    public void Add(Game newGame)
    {
        Games.Add(newGame);
    }

    public void Remove(Game game)
    {
        Games.Remove(game);
    }

    public Game SetPlayerReadyInGame(string roomId, string connectionId)
    {
        Game game = FindGameByRoomId(roomId)!;
        Player player = game.Players.Single(p => p.ConnectionId == connectionId);
        player.Ready = !player.Ready;
        return game;
    }

    public Game? DisconnectPlayerFromGame(string connectionId)
    {
        Game? game = FindGameByConnectionId(connectionId);
        if (game == null) return game;
        
        Player player = game.Players.Single(p => p.ConnectionId == connectionId);
        player.ConnectionId = null;
        return game;
    }
}