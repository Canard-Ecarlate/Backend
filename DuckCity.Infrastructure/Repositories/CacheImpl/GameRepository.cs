using DuckCity.Domain.Games;

namespace DuckCity.Infrastructure.Repositories.CacheImpl;

public class GameRepository : IGameRepository
{
    private static readonly HashSet<Game> Games = new();
    public Game FindByRoomId(string roomId)
    {
        return Games.Single(g => g.Room.Id == roomId);
    }
}