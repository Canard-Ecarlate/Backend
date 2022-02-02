using DuckCity.Domain.Users;

namespace DuckCity.GameApi.Hub;

public interface IDuckCityClient
{
    Task PushMessage(string message);
    
    Task PushPlayers(IEnumerable<Player> players);
}