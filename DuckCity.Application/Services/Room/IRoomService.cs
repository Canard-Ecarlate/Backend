namespace DuckCity.Application.Services.Room
{
    public interface IRoomService
    {
        Domain.Rooms.Room JoinRoomAndConnect(string connectionId, string userId, string userName,string roomId);
        
        Domain.Rooms.Room? LeaveRoomAndDisconnect(string roomId, string connectionId);

        Domain.Rooms.Room? DisconnectToRoom(string connectionId);
        
        Domain.Rooms.Room SetPlayerReady(string roomId, string connectionId);
    }
}