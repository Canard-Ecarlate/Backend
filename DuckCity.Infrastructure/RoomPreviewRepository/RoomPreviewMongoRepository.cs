using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using MongoDB.Driver;

namespace DuckCity.Infrastructure.RoomPreviewRepository;

public class RoomPreviewMongoRepository : IRoomPreviewRepository
{
    private readonly IMongoCollection<RoomPreview> _rooms;

    public RoomPreviewMongoRepository(IMongoDbSettings settings)
    {
        MongoClient client = new(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
        _rooms = database.GetCollection<RoomPreview>(settings.RoomsCollectionName);
    }

    public void Create(RoomPreview roomPreview)
    {
        _rooms.InsertOne(roomPreview);
    }

    public void Update(RoomPreview roomPreview) => _rooms.ReplaceOne(Builders<RoomPreview>.Filter.Eq(r => r.Id, roomPreview.Id), roomPreview);

    public RoomPreview FindById(string id)
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

    public IEnumerable<RoomPreview> FindAllRooms() => _rooms.Find(_ => true).ToList();

    public void Delete(string roomCode) => _rooms.DeleteOne(Builders<RoomPreview>.Filter.Eq(r => r.Code, roomCode));
   
    public long CountByGameContainerId(string containerId)
    {
        return _rooms.CountDocuments(Builders<RoomPreview>.Filter.Eq(r => r.ContainerId, containerId));
    }
    
    public RoomPreview FindByCode(string code)
    {
        try
        {
            return _rooms.Find(room => room.Code == code).First();
        }
        catch
        {
            throw new CodeRoomNotExistException();
        }
    }

    public bool CodeIsExist(string code)
    {
        return _rooms.Find(room => room.Code == code).CountDocuments() > 0;
    }

}