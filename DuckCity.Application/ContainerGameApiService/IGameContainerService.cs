using DuckCity.Domain;

namespace DuckCity.Application.ContainerGameApiService;

public interface IGameContainerService
{
    GameContainer ContainerAccessToCreateRoom(string roomName, string hostId);
    GameContainer ContainerAccessToJoinRoom(string roomCode, string userId);
}