using DuckCity.Domain.Games;

namespace DuckCity.Infrastructure.Repositories;

public interface IGameRepository
{
    Game? FindGameByRoomId(string gameId);
    
    void Add(Game newGame);
    
    void Remove(Game game);
    
    Game SetPlayerReadyInGame(string roomId, string connectionId);
    
    Game? DisconnectPlayerFromGame(string connectionId);
}