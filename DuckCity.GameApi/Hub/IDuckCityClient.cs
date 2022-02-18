using DuckCity.GameApi.Dto;

namespace DuckCity.GameApi.Hub;

public interface IDuckCityClient
{
    Task PushMessage(string message);
    
    Task PushPlayers(IEnumerable<PlayerInWaitingRoomDto> players);

    Task PushGame(GameDto game);
}