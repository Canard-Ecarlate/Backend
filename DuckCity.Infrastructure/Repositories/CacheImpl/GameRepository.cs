using DuckCity.Domain.Games;

namespace DuckCity.Infrastructure.Repositories.CacheImpl;

public class GameRepository : IGameRepository
{
    private static readonly HashSet<Game> Games = new();
}