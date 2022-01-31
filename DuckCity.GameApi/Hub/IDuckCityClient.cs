using DuckCity.Domain.Rooms;

namespace DuckCity.GameApi.Hub;

public interface IDuckCityClient
{
    Task PushMessage(string message);
    
    Task PushPlayersInRoom(IEnumerable<PlayerInRoom> players);
    
    Task PushPlayersInSignalRGroup(IEnumerable<SignalRUser> users);
}