using DuckCity.Domain.Rooms;

namespace DuckCity.Application.RoomService;

public interface IRoomService
{
    Room JoinRoomAndConnect(string connectionId, string userId, string userName,string roomId);
        
    Room? LeaveRoomAndDisconnect(string roomId, string connectionId);

    Room? DisconnectFromRoom(string connectionId);
        
    Room SetPlayerReady(string roomId, string connectionId);
}