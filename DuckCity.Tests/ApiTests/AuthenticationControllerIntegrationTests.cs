using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using DuckCity.Api.DTO.Authentication;
using DuckCity.Tests.UtilsTests;
using Xunit;

namespace DuckCity.Tests.ApiTests;

public sealed class AuthenticationControllerIntegrationTests : IClassFixture<MongoDbFake>
{
    private readonly HttpClient _client;

    public AuthenticationControllerIntegrationTests(MongoDbFake mongoDbFake)
    {
        _client = new ApiApplication(mongoDbFake).CreateClient();
        mongoDbFake.Dispose();
    }
    
    [Fact]
    public async Task SignUpTest()
    {
        RegisterDto registerDto = new()
            {Name = "string", Email = "e@e.fr", Password = "string", PasswordConfirmation = "string"};
        JsonContent jsonIn = JsonContent.Create(registerDto);
        HttpResponseMessage signUp = await _client.PostAsync("/api/Authentication/SignUp", jsonIn);
        string contentSignUp = signUp.Content.ReadAsStringAsync().Result;
        UserWithTokenDto? userSignUp = JsonSerializer.Deserialize<UserWithTokenDto>(contentSignUp,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

        Assert.Equal("string", userSignUp?.Name);
        Assert.Equal("e@e.fr", userSignUp?.Email);
        Assert.NotNull(userSignUp?.Id);
        Assert.NotNull(userSignUp?.Token);
    }

    [Fact]
    public async Task LoginTest()
    {
        RegisterDto registerDto = new()
            {Name = "string", Email = "e@e.fr", Password = "string", PasswordConfirmation = "string"};
        JsonContent jsonIn = JsonContent.Create(registerDto);
        await _client.PostAsync("/api/Authentication/SignUp", jsonIn);

        IdentifierDto identifierDto = new() {Name = "string", Password = "string"};
        JsonContent json = JsonContent.Create(identifierDto);
        HttpResponseMessage login = await _client.PostAsync("/api/Authentication/Login", json);
        string contentLogin = login.Content.ReadAsStringAsync().Result;
        UserWithTokenDto? userLogin = JsonSerializer.Deserialize<UserWithTokenDto>(contentLogin,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

        Assert.Equal("string", userLogin?.Name);
        Assert.Equal("e@e.fr", userLogin?.Email);
        Assert.NotNull(userLogin?.Id);
        Assert.NotNull(userLogin?.Token);
    }
}