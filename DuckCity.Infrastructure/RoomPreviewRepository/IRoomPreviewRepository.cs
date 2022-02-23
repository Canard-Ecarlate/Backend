using DuckCity.Domain.Rooms;

namespace DuckCity.Infrastructure.RoomPreviewRepository;

public interface IRoomPreviewRepository
{
    void Create(RoomPreview roomPreview);
        
    RoomPreview FindById(string id);

    IEnumerable<RoomPreview> FindAllRooms();
    
    RoomPreview? FindByUserId(string userId);

    void Update(RoomPreview roomPreview);

    void Delete(string roomCode);
    
    long CountByGameContainerId(string containerId);

    RoomPreview FindByCode(string code);

    bool CodeExists(string code);
}