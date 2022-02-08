using DuckCity.Domain.Exceptions;
using MongoDB.Driver;

namespace DuckCity.Infrastructure.Repositories.RoomPreview;

public class RoomPreviewMongoRepository : IRoomPreviewRepository
{
    private readonly IMongoCollection<Domain.Rooms.RoomPreview> _rooms;

    public RoomPreviewMongoRepository(IMongoDbSettings settings)
    {
        MongoClient client = new(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
        _rooms = database.GetCollection<Domain.Rooms.RoomPreview>(settings.RoomsCollectionName);
    }

    public void Create(Domain.Rooms.RoomPreview roomPreview)
    {
        _rooms.InsertOne(roomPreview);
    }

    public void Replace(Domain.Rooms.RoomPreview roomPreview) => _rooms.ReplaceOne(Builders<Domain.Rooms.RoomPreview>.Filter.Eq(r => r.Id, roomPreview.Id), roomPreview);

    public Domain.Rooms.RoomPreview FindById(string id)
    {
        try
        {
            return _rooms.Find(room => room.Id == id).First();
        }
        catch
        {
            throw new RoomNotFoundException(id);
        }
    }

    public IEnumerable<Domain.Rooms.RoomPreview> FindAllRooms() => _rooms.Find(_ => true).ToList();

    public void Delete(Domain.Rooms.RoomPreview roomPreview) => _rooms.DeleteOne(Builders<Domain.Rooms.RoomPreview>.Filter.Eq(r => r.Id, roomPreview.Id));
}