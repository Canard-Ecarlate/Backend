using DuckCity.Domain.Users;
using DuckCity.Infrastructure.UserRepository;
using DuckCity.Tests.Fake;
using Xunit;

namespace DuckCity.Tests.IntegrationTests.Infrastructure
{
    public class UserRepositoryIt : IClassFixture<MongoDbFake>
    {
        private readonly MongoDbFake _mongoDbFake;

        // Constructor
        public UserRepositoryIt(MongoDbFake mongoDbFake)
        {
            _mongoDbFake = mongoDbFake;
        }

        /**
         * Tests
         */
        [Fact]
        public void CreateTest()
        {
            UserMongoRepository userMongoRepository = new(_mongoDbFake.MongoSettings);
            userMongoRepository.Create(new User("", "", "")
            {
                Id = ConstantTest.UserId
            });
            User user = userMongoRepository.FindById(ConstantTest.UserId);
            Assert.NotNull(user);
        }
    }
}