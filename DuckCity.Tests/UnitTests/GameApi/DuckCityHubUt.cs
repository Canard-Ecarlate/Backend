using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Rooms;
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
        private readonly Mock<IDuckCityClient> _mockDuckCityClient = new();

        // Constructor
        public DuckCityHubUt()
        {
            _duckCityHub = new DuckCityHub(_mockRoomService.Object)
            {
                Context = _mockHubContext.Object,
                Clients = _mockClients.Object,
                Groups = _mockGroups.Object
            };
            _mockHubContext.Setup(context => context.ConnectionId).Returns(ConstantTest.ObjectId1);
            _mockClients.Setup(clients => clients.All).Returns(_mockDuckCityClient.Object);
        }

        /**
         * Tests
         */
        [Theory]
        [InlineData(ConstantTest.ObjectId1, ConstantTest.ObjectId1)]
        public async Task JoinSignalRGroupAsyncTest(string userId, string roomId)
        {
            // Given
            Room room = new(roomId, userId, userId, true, 5)
            {
                Id = roomId
            };
            HashSet<PlayerInRoom> players = room.Players;
            
            // Mock
            _mockRoomService.Setup(service => service.FindRoom(roomId)).Returns(room);
            _mockClients.Setup(clients => clients.Group(roomId)).Returns(_mockDuckCityClient.Object);

            //When
                await _duckCityHub.JoinSignalRGroupAsync(roomId, userId);
                
                // Then
                Assert.True(players.First(p => p.Id == userId).Connected);

                // Verify
                _mockRoomService.Verify(service => service.FindRoom(roomId), Times.Once);
                _mockClients.Verify(clients => clients.Group(roomId), Times.Once);
                _mockDuckCityClient.Verify(duck => duck.PushPlayersInRoom(players), Times.Once);
        }

        [Theory]
        [InlineData(ConstantTest.ObjectId1, ConstantTest.ObjectId1, ConstantTest.ObjectId2)]
        public async Task JoinSignalRGroupAsyncTwoPlayersTest(string userId, string roomId, string userId2)
        {
            // Given
            Room room = new(roomId, userId, userId, true, 5)
            {
                Id = roomId
            };
            HashSet<PlayerInRoom> players = room.Players;
            players.Add(new PlayerInRoom {Id = userId2});

            // Mock
            _mockRoomService.Setup(service => service.FindRoom(roomId)).Returns(room);
            _mockClients.Setup(clients => clients.Group(roomId)).Returns(_mockDuckCityClient.Object);

            //When
            await _duckCityHub.JoinSignalRGroupAsync(roomId, userId);
            await _duckCityHub.JoinSignalRGroupAsync(roomId, userId2);

            // Then
            Assert.True(players.First(p => p.Id == userId).Connected);
            Assert.True(players.First(p => p.Id == userId2).Connected);

            // Verify
            _mockRoomService.Verify(service => service.FindRoom(roomId), Times.Exactly(2));
            _mockClients.Verify(clients => clients.Group(roomId), Times.Exactly(2));
            _mockDuckCityClient.Verify(duck => duck.PushPlayersInRoom(players), Times.Exactly(2));
        }
    }
}