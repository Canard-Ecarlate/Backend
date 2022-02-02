// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using DuckCity.Application.Services.Interfaces;
// using DuckCity.Domain.Rooms;
// using DuckCity.GameApi.Hub;
// using Microsoft.AspNetCore.SignalR;
// using Moq;
// using SignalR_UnitTestingSupportXUnit.Hubs;
// using Xunit;
// using Xunit.Sdk;
//
// namespace DuckCity.Tests.UnitTests.GameApi
// {
//     public class DuckCityHubUt : HubUnitTestsBase
//     {
//         // Class to test
//         private readonly DuckCityHub _duckCityHub;
//
//         // Mock
//         private readonly Mock<IRoomService> _mockRoomService = new();
//         private readonly Mock<HubCallerContext> _mockHubContext = new();
//         private readonly Mock<IHubCallerClients<IDuckCityClient>> _mockClients = new();
//         private readonly Mock<IGroupManager> _mockGroups = new();
//         private readonly Mock<IDuckCityClient> _mockDuckCityClient = new();
//
//         // Constructor
//         public DuckCityHubUt()
//         {
//             _duckCityHub = new DuckCityHub(_mockRoomService.Object)
//             {
//                 Context = _mockHubContext.Object,
//                 Clients = _mockClients.Object,
//                 Groups = _mockGroups.Object
//             };
//             _mockHubContext.Setup(context => context.ConnectionId).Returns(ConstantTest.ConnectionId);
//             _mockClients.Setup(clients => clients.All).Returns(_mockDuckCityClient.Object);
//             PlayersCache.ListAllPlayers.Clear();
//         }
//
//         /**
//          * Tests
//          */
//         [Theory]
//         [InlineData(ConstantTest.UserId, ConstantTest.RoomId)]
//         public async Task JoinSignalRGroupAsyncTest(string userId, string roomId)
//         {
//             // Given
//             Room room = new(roomId, userId, userId, true, 5)
//             {
//                 Id = roomId
//             };
//             HashSet<PlayerInRoom> players = room.PlayersId;
//             
//             // Mock
//             _mockRoomService.Setup(service => service.FindRoom(roomId)).Returns(room);
//             _mockClients.Setup(clients => clients.Group(roomId)).Returns(_mockDuckCityClient.Object);
//
//             //When
//                 await _duckCityHub.ConnectToRoomAsync(roomId, userId);
//                 
//                 // Then
//                 Assert.True(players.First(p => p.Id == userId).Connected);
//
//                 // Verify
//                 _mockRoomService.Verify(service => service.FindRoom(roomId), Times.Once);
//                 _mockClients.Verify(clients => clients.Group(roomId), Times.Once);
//                 _mockDuckCityClient.Verify(duck => duck.PushPlayers(players), Times.Once);
//         }
//
//         [Theory]
//         [InlineData(ConstantTest.UserId, ConstantTest.RoomId, ConstantTest.UserId2)]
//         public async Task JoinSignalRGroupAsyncTwoPlayersTest(string userId, string roomId, string userId2)
//         {
//             // Given
//             Room room = new(roomId, userId, userId, true, 5)
//             {
//                 Id = roomId
//             };
//             HashSet<PlayerInRoom> players = room.PlayersId;
//             players.Add(new PlayerInRoom {Id = userId2});
//
//             // Mock
//             _mockRoomService.Setup(service => service.FindRoom(roomId)).Returns(room);
//             _mockClients.Setup(clients => clients.Group(roomId)).Returns(_mockDuckCityClient.Object);
//
//             //When
//             await _duckCityHub.ConnectToRoomAsync(roomId, userId);
//             await _duckCityHub.ConnectToRoomAsync(roomId, userId2);
//
//             // Then
//             Assert.True(players.First(p => p.Id == userId).Connected);
//             Assert.True(players.First(p => p.Id == userId2).Connected);
//
//             // Verify
//             _mockRoomService.Verify(service => service.FindRoom(roomId), Times.Exactly(2));
//             _mockClients.Verify(clients => clients.Group(roomId), Times.Exactly(2));
//             _mockDuckCityClient.Verify(duck => duck.PushPlayers(players), Times.Exactly(2));
//         }
//         
//         [Theory]
//         [InlineData(ConstantTest.ConnectionId, ConstantTest.UserId, ConstantTest.RoomId)]
//         public async Task LeaveRoomAndRoomDeletedTest(string connectionId, string userId, string roomId)
//         {
//             // Given
//             SignalRUser signalRUser = new() {ConnectionId = connectionId, RoomId = roomId, UserId = userId};
//             PlayersCache.ListAllPlayers.Add(signalRUser);
//             Room? room = null;
//             
//             // Mock
//             _mockRoomService.Setup(mock => mock.LeaveRoom(roomId, userId)).Returns(room);
//             _mockClients.Setup(clients => clients.Group(roomId)).Returns(_mockDuckCityClient.Object);
//
//             // When
//             await _duckCityHub.LeaveRoomAsync();
//
//             // Then
//             Assert.Empty(PlayersCache.ListAllPlayers);
//             _mockRoomService.Verify(mock => mock.LeaveRoom(roomId, userId), Times.Once);
//             _mockGroups.Verify(mock => mock.RemoveFromGroupAsync(connectionId, roomId, CancellationToken.None), Times.Once);
//             _mockDuckCityClient.Verify(duck => duck.PushPlayers(It.IsAny<HashSet<PlayerInRoom>>()), Times.Never);
//         }
//         
//         [Theory]
//         [InlineData(ConstantTest.ConnectionId, ConstantTest.UserId, ConstantTest.RoomId, ConstantTest.ConnectionId2, ConstantTest.UserId2)]
//         public async Task LeaveRoomAndRoomKeptTest(string connectionId, string userId, string roomId, string connectionId2, string userId2)
//         {
//             // Given
//             SignalRUser signalRUser = new() {ConnectionId = connectionId, RoomId = roomId, UserId = userId};
//             SignalRUser signalRUser2 = new() {ConnectionId = connectionId2, RoomId = roomId, UserId = userId2};
//             PlayersCache.ListAllPlayers.Add(signalRUser);
//             PlayersCache.ListAllPlayers.Add(signalRUser2);
//             Room room = new("roomName", userId2, userId2, true, 5)
//             {
//                 Id = roomId
//             };
//             
//             // Mock
//             _mockRoomService.Setup(mock => mock.LeaveRoom(roomId, userId)).Returns(room);
//             _mockClients.Setup(clients => clients.Group(roomId)).Returns(_mockDuckCityClient.Object);
//
//             // When
//             await _duckCityHub.LeaveRoomAsync();
//
//             // Then
//             Assert.NotEmpty(PlayersCache.ListAllPlayers);
//             Assert.True(room.PlayersId.First(p => p.Id == userId2).Connected);
//             
//             // Verify
//             _mockRoomService.Verify(mock => mock.LeaveRoom(roomId, userId), Times.Once);
//             _mockGroups.Verify(mock => mock.RemoveFromGroupAsync(connectionId, roomId, CancellationToken.None), Times.Once);
//             _mockDuckCityClient.Verify(duck => duck.PushPlayers(room.PlayersId), Times.Once);
//         }
//
//         [Theory]
//         [InlineData(ConstantTest.RoomId, ConstantTest.ConnectionId, ConstantTest.UserId)]
//         public async Task OnDisconnectedAsyncTest(string roomId, string connectionId, string userId)
//         {
//             // Given
//             SignalRUser signalRUser = new() {ConnectionId = connectionId, RoomId = roomId, UserId = userId};
//             PlayersCache.ListAllPlayers.Add(signalRUser);
//
//             // Mock
//             _mockClients.Setup(clients => clients.Group(roomId)).Returns(_mockDuckCityClient.Object);
//
//             // When
//             await _duckCityHub.OnDisconnectedAsync(new Exception());
//             
//             // Then
//             Assert.Empty(PlayersCache.ListAllPlayers);
//             
//             // Verify
//             _mockGroups.Verify(mock => mock.RemoveFromGroupAsync(connectionId, roomId, CancellationToken.None), Times.Once);
//             _mockDuckCityClient.Verify(duck => duck.PushPlayersInSignalRGroup(It.IsAny<IEnumerable<SignalRUser>>()), Times.Once);
//         }
//         
//         [Theory]
//         [InlineData(ConstantTest.RoomId, ConstantTest.ConnectionId, ConstantTest.UserId, ConstantTest.ConnectionId2, ConstantTest.UserId2)]
//         public async Task OnDisconnectedAsyncStillOnePlayerConnectedTest(string roomId, string connectionId, string userId, string connectionId2, string userId2)
//         {
//             // Given
//             SignalRUser signalRUser = new() {ConnectionId = connectionId, RoomId = roomId, UserId = userId};
//             SignalRUser signalRUser2 = new() {ConnectionId = connectionId2, RoomId = roomId, UserId = userId2};
//             PlayersCache.ListAllPlayers.Add(signalRUser);
//             PlayersCache.ListAllPlayers.Add(signalRUser2);
//
//             // Mock
//             _mockClients.Setup(clients => clients.Group(roomId)).Returns(_mockDuckCityClient.Object);
//
//             // When
//             await _duckCityHub.OnDisconnectedAsync(new Exception());
//            
//             // Then
//             Assert.NotEmpty(PlayersCache.ListAllPlayers);
//             Assert.Single(PlayersCache.ListAllPlayers);
//             
//             // Verify
//             _mockGroups.Verify(mock => mock.RemoveFromGroupAsync(connectionId, roomId, CancellationToken.None), Times.Once);
//             _mockDuckCityClient.Verify(duck => duck.PushPlayersInSignalRGroup(It.IsAny<IEnumerable<SignalRUser>>()), Times.Once);
//         }
//
//         [Fact]
//         public async Task OnConnectedAsync()
//         { 
//             try
//             {
//                 // When
//                 await _duckCityHub.OnConnectedAsync();
//             }
//             catch (Exception ex)
//             {
//                 throw new XunitException(ex.Message);
//             }
//         }
//     }
// }