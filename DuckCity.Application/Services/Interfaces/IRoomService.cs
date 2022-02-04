using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;

namespace DuckCity.Application.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<Room> FindAllRooms();
        Room FindRoom(string roomId);
        Room CreateAndJoinRoom(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers);
        Room JoinRoom(string roomId, string userId, string userName);
        void ConnectOrReconnectPlayer(string connectionId, string userId, string userName,string roomId);
        string? DisconnectPlayerAndLeaveRoom(string connectionId);
        IEnumerable<Player> FindPlayersInRoom(string roomId);
        string PlayerReady(string connectionId);
        string? DisConnectToRoom(string connectionId);
    }
}