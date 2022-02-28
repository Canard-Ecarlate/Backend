using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using MongoDB.Driver;

namespace DuckCity.Infrastructure.RoomPreviewRepository;

public class RoomPreviewMongoRepository : IRoomPreviewRepository
{
    private readonly IMongoCollection<RoomPreview> _roomsPreview;

    public RoomPreviewMongoRepository(IMongoDbSettings settings)
    {
        MongoClient client = new(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
        _roomsPreview = database.GetCollection<RoomPreview>(settings.RoomsCollectionName);
    }

    public void Create(RoomPreview roomPreview)
    {
        _roomsPreview.InsertOne(roomPreview);
    }
    
    public RoomPreview? FindByUserId(string userId)
    {
        return _roomsPreview.Find(r => r.PlayersId.Contains(userId)).FirstOrDefault();
    }

    public void Update(RoomPreview roomPreview) => _roomsPreview.ReplaceOne(Builders<RoomPreview>.Filter.Eq(r => r.Id, roomPreview.Id), roomPreview);

    public RoomPreview FindById(string id)
    {
        try
        {
            return _roomsPreview.Find(room => room.Id == id).First();
        }
        catch
        {
            throw new RoomNotFoundException(id);
        }
    }

    public IEnumerable<RoomPreview> FindAllRooms() => _roomsPreview.Find(_ => true).ToList();

    public void Delete(string roomCode) => _roomsPreview.DeleteOne(Builders<RoomPreview>.Filter.Eq(r => r.Code, roomCode));
   
    public long CountByGameContainerId(string containerId)
    {
        return _roomsPreview.CountDocuments(Builders<RoomPreview>.Filter.Eq(r => r.ContainerId, containerId));
    }
    
    public RoomPreview? FindByCode(string code)
    {
        return _roomsPreview.Find(room => room.Code == code).FirstOrDefault();
    }

    public bool CodeExists(string code)
    {
        return _roomsPreview.Find(room => room.Code == code).CountDocuments() > 0;
    }

}