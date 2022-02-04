using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;

namespace DuckCity.Application.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<Room> FindAllRooms();
        Room FindRoom(string roomId);
        Room CreateRoom(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers);
        Room JoinRoom(string roomId, string userId, string userName);
        void ConnectOrReconnectToRoom(string connectionId, string userId, string userName,string roomId);
        string? LeaveAndDisconnectRoom(string connectionId);
        IEnumerable<Player> FindPlayersInRoom(string roomId);
        string SetReadyToPlayer(string connectionId);
        string? DisConnectToRoom(string connectionId);
    }
}