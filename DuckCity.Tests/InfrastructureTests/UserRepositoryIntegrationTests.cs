using System.Collections.Generic;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Repositories;
using DuckCity.Tests.UtilsTests;
using Xunit;

namespace DuckCity.Tests.InfrastructureTests;

public class UserRepositoryIntegrationTests : IClassFixture<MongoDbFake>
{
    private readonly MongoDbFake _fixture;

    public UserRepositoryIntegrationTests(MongoDbFake fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void CreateTest()
    {
        UserRepository userRepository = new(_fixture.MongoSettings);
        userRepository.Create(new User{Id = "61e1c881254c966b1ecd9c86"});
        IList<User> user = userRepository.GetById("61e1c881254c966b1ecd9c86");
        Assert.NotEmpty(user);
    }
}