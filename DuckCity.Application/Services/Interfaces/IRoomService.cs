using DuckCity.Domain.Rooms;

namespace DuckCity.Application.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<Room> FindAllRooms();

        Room FindRoom(string roomId);

        Room AddRooms(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers);

        Room JoinRoom(string roomId, string userId, string userName);

        Room UpdatedRoomReady(string userId, string roomId);

        Room? LeaveRoom(string roomId, string userId);
    }
}