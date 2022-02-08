using DuckCity.Domain.Rooms;

namespace DuckCity.Infrastructure.RoomPreviewRepository
{
    public interface IRoomPreviewRepository
    {
        void Create(RoomPreview roomPreview);
        
        void Replace(RoomPreview roomPreview);

        void Delete(RoomPreview roomPreview);
        
        RoomPreview FindById(string id);

        IEnumerable<RoomPreview> FindAllRooms();
    }
}