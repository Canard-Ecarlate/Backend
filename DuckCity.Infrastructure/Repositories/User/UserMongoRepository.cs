using MongoDB.Driver;

namespace DuckCity.Infrastructure.Repositories.User
{
    public class UserMongoRepository : IUserRepository
    {
        private readonly IMongoCollection<Domain.Users.User> _users;

        public UserMongoRepository(IMongoDbSettings settings)
        {
            MongoClient client = new(settings.ConnectionString);
            IMongoDatabase? database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<Domain.Users.User>(settings.UsersCollectionName);
        }

        public IList<Domain.Users.User> GetByName(string? name) =>
            _users.Find(user => user.Name == name).ToList();

        public IList<Domain.Users.User> GetById(string? id) =>
            _users.Find(user => user.Id == id).ToList();

        public void Create(Domain.Users.User user)
        {
            _users.InsertOne(user);
        }

        public long CountUserByName(string? name) =>
            _users.Find(user => user.Name == name).CountDocuments();

        public long CountUserById(string? id) =>
            _users.Find(user => user.Id == id).CountDocuments();

        public long CountUserByEmail(string? email) =>
            _users.Find(user => user.Email == email).CountDocuments();
    }
}