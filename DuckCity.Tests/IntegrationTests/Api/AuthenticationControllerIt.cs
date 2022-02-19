using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using DuckCity.Api.DTO.Authentication;
using DuckCity.Tests.Fake;
using Xunit;

namespace DuckCity.Tests.IntegrationTests.Api
{
    public sealed class AuthenticationControllerIt : IClassFixture<MongoDbFake>
    {
        private readonly HttpClient _client;

        // Constructor
        public AuthenticationControllerIt(MongoDbFake mongoDbFake)
        {
            _client = new ApiApplicationFake(mongoDbFake).CreateClient();
            mongoDbFake.Dispose();
        }

        /**
         * Tests
         */
        [Theory]
        [InlineData(ConstantTest.String, ConstantTest.Email, ConstantTest.String)]
        public async Task SignUpTest(string name, string email, string password)
        {
            RegisterDto registerDto = new()
            {
                Name = name, Email = email, Password = password, PasswordConfirmation = password
            };
            JsonContent jsonIn = JsonContent.Create(registerDto);
            
            // When
            HttpResponseMessage signUp = await _client.PostAsync("/api/Authentication/SignUp", jsonIn);
            
            // Then
            string contentSignUp = signUp.Content.ReadAsStringAsync().Result;
            TokenAndCurrentContainerIdDto? userSignUp = JsonSerializer.Deserialize<TokenAndCurrentContainerIdDto>(contentSignUp,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            Assert.NotNull(userSignUp?.Token);
        }

        [Theory]
        [InlineData(ConstantTest.String, ConstantTest.Email, ConstantTest.String)]
        public async Task LoginTest(string name, string email, string password)
        {
            RegisterDto registerDto = new()
            {
                Name = name, Email = email, Password = password, PasswordConfirmation = password
            };
            JsonContent jsonIn = JsonContent.Create(registerDto);
            await _client.PostAsync("/api/Authentication/SignUp", jsonIn);

            IdentifierDto identifierDto = new() {Name = name, Password = password};
            JsonContent json = JsonContent.Create(identifierDto);
            HttpResponseMessage login = await _client.PostAsync("/api/Authentication/Login", json);
            string contentLogin = login.Content.ReadAsStringAsync().Result;
            TokenAndCurrentContainerIdDto? userLogin = JsonSerializer.Deserialize<TokenAndCurrentContainerIdDto>(contentLogin,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            Assert.NotNull(userLogin?.Token);
        }
    }
}