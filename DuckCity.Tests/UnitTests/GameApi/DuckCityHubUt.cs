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
        // Class to test
        private readonly DuckCityHub _duckCityHub;
        
        // Mock
        private readonly Mock<IRoomService> _mockRoomService = new();
        private readonly Mock<HubCallerContext> _mockHubContext = new();
        private readonly Mock<IHubCallerClients<IDuckCityClient>> _mockClients = new();
        private readonly Mock<IGroupManager> _mockGroups = new();
        private readonly Mock<IDuckCityClient> _mockClientProxy = new();

        // Constructor
        public DuckCityHubUt()
        {
            _duckCityHub = new DuckCityHub(_mockRoomService.Object)
            {
                Context = _mockHubContext.Object,
                Clients = _mockClients.Object,
                Groups = _mockGroups.Object
            };
            _mockClients.Setup(clients => clients.All).Returns(_mockClientProxy.Object);
        }

        /**
         * Tests
         */
        [Fact]
        public async Task SendNotification()
        {
            //When
            await _duckCityHub.SendMessageHubAsync("Yo! This is the unit test.");
            
            // Verify
            _mockClients.Verify(clients => clients.All, Times.Once);
        }
    }
}