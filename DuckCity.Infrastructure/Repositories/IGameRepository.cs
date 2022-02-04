using DuckCity.Domain.Games;

namespace DuckCity.Infrastructure.Repositories;

public interface IGameRepository
{
    Game FindByRoomId(string gameId);
}