namespace DuckCity.Application.Services.RoomPreview;

public interface IRoomPreviewService
{
    Domain.Rooms.RoomPreview CreateAndJoinRoomPreview(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers);
    
    Domain.Rooms.RoomPreview JoinRoomPreview(string roomId, string userId);
    
    Domain.Rooms.RoomPreview? LeaveRoomPreview(string roomId, string userId);
    
    IEnumerable<Domain.Rooms.RoomPreview> FindAllRooms();
}