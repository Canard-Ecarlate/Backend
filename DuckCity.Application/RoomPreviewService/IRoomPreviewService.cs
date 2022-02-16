using DuckCity.Domain.Rooms;

namespace DuckCity.Application.RoomPreviewService;

public interface IRoomPreviewService
{
    IEnumerable<RoomPreview> FindAllRooms();
    
    void CreateRoomPreview(RoomPreview newRoomPreview);

    void UpdateRoomPreview(RoomPreview roomPreview);
    
    void DeleteRoomPreview(string roomId);
}