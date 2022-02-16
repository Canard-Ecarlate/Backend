using DuckCity.Domain;
using MongoDB.Driver;

namespace DuckCity.Infrastructure.GameContainerRepository;

public class GameContainerMongoRepository : IGameContainerRepository
{
    private readonly IMongoCollection<GameContainer> _gameContainer;

    public GameContainerMongoRepository(IMongoDbSettings settings)
    {
        MongoClient client = new(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
        _gameContainer = database.GetCollection<GameContainer>(settings.GameContainersCollectionName);
    }

    public void Create(GameContainer gameContainer)
    {
        _gameContainer.InsertOne(gameContainer);
    }

    public GameContainer FindById(string gameContainerId)
    {
        return _gameContainer.Find(g => g.Id == gameContainerId).First();
    }

    public GameContainer FindByNumber(int number)
    {
        return _gameContainer.Find(g => g.Number == number).First();
    }

    public void Update(GameContainer gameContainer)
    {
        _gameContainer.ReplaceOne(Builders<GameContainer>.Filter.Eq(g => g.Id, gameContainer.Id), gameContainer);
    }

    public void Delete(string gameContainerId)
    {
        _gameContainer.DeleteOne(Builders<GameContainer>.Filter.Eq(g => g.Id, gameContainerId));
    }
}