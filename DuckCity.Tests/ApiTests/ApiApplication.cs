using DuckCity.Api;
using DuckCity.Infrastructure;
using DuckCity.Tests.UtilsTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace DuckCity.Tests.ApiTests
{
    public class ApiApplication : WebApplicationFactory<ProgramApi>
    {
        private readonly MongoDbFake _mongoDbFake;

        public ApiApplication(MongoDbFake mongoDbFake)
        {
            _mongoDbFake = mongoDbFake;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IMongoDbSettings>(sp => _mongoDbFake.MongoSettings);
            });
        }
    }
}