using DuckCity.GameApi.Dto;

namespace DuckCity.GameApi.Hub;

public interface IDuckCityClient
{
    Task PushRoom(RoomDto roomDto); // ne pas envoyer game comme ça
    
    Task PushPlayers(IEnumerable<PlayerInWaitingRoomDto> players);

    Task PushGame(GameDto game);
}