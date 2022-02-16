using DuckCity.Domain.Rooms;

namespace DuckCity.Application.RoomService;

public interface IRoomService
{
    void CreateRoom(Room newRoom);
    
    Room JoinRoom(string connectionId, string userId, string userName,string roomId);

    Room? LeaveRoom(string roomId, string connectionId);

    Room? DisconnectFromRoom(string connectionId);

    Room SetPlayerReady(string roomId, string connectionId);
}