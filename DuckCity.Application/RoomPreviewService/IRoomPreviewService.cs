using DuckCity.Domain.Rooms;

namespace DuckCity.Application.RoomPreviewService;

public interface IRoomPreviewService
{
    void CreateRoomPreview(RoomPreview newRoomPreview);

    string GenerateCode();
    
    IEnumerable<RoomPreview> FindAllRooms();

    RoomPreview? FindByUserId(string userId);

    void UpdateRoomPreview(RoomPreview roomPreview);

    void DeleteRoomPreview(string roomCode);
    
    void DeleteRoomPreviewByUserId(string userId);
}