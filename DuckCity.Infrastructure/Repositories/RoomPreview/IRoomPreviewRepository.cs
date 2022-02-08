namespace DuckCity.Infrastructure.Repositories.RoomPreview
{
    public interface IRoomPreviewRepository
    {
        void Create(Domain.Rooms.RoomPreview roomPreview);
        
        void Replace(Domain.Rooms.RoomPreview roomPreview);

        void Delete(Domain.Rooms.RoomPreview roomPreview);
        
        Domain.Rooms.RoomPreview FindById(string id);

        IEnumerable<Domain.Rooms.RoomPreview> FindAllRooms();
    }
}