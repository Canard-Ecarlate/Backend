using System.Collections.Generic;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Repositories;
using DuckCity.Infrastructure.Repositories.MongoImpl;
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
            UserRepository userRepository = new(_mongoDbFake.MongoSettings);
            userRepository.Create(new User {Id = "61e1c881254c966b1ecd9c86"});
            IList<User> user = userRepository.GetById("61e1c881254c966b1ecd9c86");
            Assert.NotEmpty(user);
        }
    }
}