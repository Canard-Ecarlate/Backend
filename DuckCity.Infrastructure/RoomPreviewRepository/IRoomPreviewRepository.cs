using DuckCity.Domain.Rooms;

namespace DuckCity.Infrastructure.RoomPreviewRepository;

public interface IRoomPreviewRepository
{
    void Create(RoomPreview roomPreview);
        
    RoomPreview FindById(string id);

    IEnumerable<RoomPreview> FindAllRooms();

    void Update(RoomPreview roomPreview);

    void Delete(string roomId);
    
    long CountByGameContainerId(string containerId);
}