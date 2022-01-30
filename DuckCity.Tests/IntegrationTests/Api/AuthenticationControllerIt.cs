using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using DuckCity.Api.DTO.Authentication;
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
            HttpResponseMessage signUp = await _client.PostAsync("/api/Authentication/SignUp", jsonIn);
            string contentSignUp = signUp.Content.ReadAsStringAsync().Result;
            UserWithTokenDto? userSignUp = JsonSerializer.Deserialize<UserWithTokenDto>(contentSignUp,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            Assert.Equal(name, userSignUp?.Name);
            Assert.Equal(email, userSignUp?.Email);
            Assert.NotNull(userSignUp?.Id);
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
            UserWithTokenDto? userLogin = JsonSerializer.Deserialize<UserWithTokenDto>(contentLogin,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            Assert.Equal(name, userLogin?.Name);
            Assert.Equal(email, userLogin?.Email);
            Assert.NotNull(userLogin?.Id);
            Assert.NotNull(userLogin?.Token);
        }
    }
}