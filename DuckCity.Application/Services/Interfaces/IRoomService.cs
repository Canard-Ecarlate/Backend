using DuckCity.Domain.Games;
using DuckCity.Domain.Rooms;

namespace DuckCity.Application.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<Room> FindAllRooms();
        
        Room FindRoom(string roomId);
        
        Room CreateAndJoinRoom(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers);
        
        Room JoinRoom(string roomId, string userId, string userName);
        
        Game JoinGameAndConnect(string connectionId, string userId, string userName,string roomId);
        
        Game? LeaveGameAndDisconnect(string roomId, string connectionId);
        
        Game PlayerReady(string roomId, string connectionId);
        
        Game? DisConnectToRoom(string connectionId);
    }
}