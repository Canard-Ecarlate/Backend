using DuckCity.Domain.Rooms;

namespace DuckCity.Application.RoomPreviewService;

public interface IRoomPreviewService
{
    RoomPreview CreateAndJoinRoomPreview(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers);
    
    RoomPreview JoinRoomPreview(string roomId, string userId);
    
    RoomPreview? LeaveRoomPreview(string roomId, string userId);
    
    IEnumerable<Domain.Rooms.RoomPreview> FindAllRooms();
}