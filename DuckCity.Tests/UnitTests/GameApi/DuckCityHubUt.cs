using System.Threading.Tasks;
using DuckCity.Application.Services.Interfaces;
using DuckCity.GameApi.Hub;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SignalR_UnitTestingSupportXUnit.Hubs;
using Xunit;

namespace DuckCity.Tests.UnitTests.GameApi
{
    public class DuckCityHubUt : HubUnitTestsBase
    {
        // Mock
        private readonly Mock<IRoomService> _mockRoomService = new();

        [Fact]
        public async Task SendNotification()
        {
            Mock<HubCallerContext> mockHubContext = new();
            Mock<IHubCallerClients> mockClients = new();
            Mock<IClientProxy> mockClientProxy = new();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);


            DuckCityHub hub = new(_mockRoomService.Object);
            hub.Context = mockHubContext.Object;
            hub.Clients = mockClients.Object;

            await hub.SendMessageHubAsync("Yo! This is the unit test.");

            mockClients.Verify(clients => clients.All, Times.Once);
        }
    }
}