using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DuckCity.Application.RoomPreviewService;
using DuckCity.Application.RoomService;
using DuckCity.Domain.Rooms;
using DuckCity.GameApi.Dto;
using DuckCity.GameApi.Hub;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SignalR_UnitTestingSupportXUnit.Hubs;
using Xunit;

namespace DuckCity.Tests.UnitTests.GameApi;

public class DuckCityHubUt : HubUnitTestsBase
{
    // Class to test
    private readonly DuckCityHub _duckCityHub;

    // Mock
    private readonly Mock<IRoomService> _mockRoomService = new();
    private readonly Mock<IMapper> _mockMapper = new();
    private readonly Mock<IRoomPreviewService> _mockRoomPreviewService = new();

    private readonly Mock<HubCallerContext> _mockHubContext = new();
    private readonly Mock<IHubCallerClients<IDuckCityClient>> _mockClients = new();
    private readonly Mock<IGroupManager> _mockGroups = new();
    private readonly Mock<IDuckCityClient> _mockDuckCityClient = new();

    // Constructor
    public DuckCityHubUt()
    {
        _duckCityHub = new DuckCityHub(_mockRoomService.Object, _mockMapper.Object, _mockRoomPreviewService.Object)
        {
            Context = _mockHubContext.Object,
            Clients = _mockClients.Object,
            Groups = _mockGroups.Object
        };
        _mockHubContext.Setup(context => context.ConnectionId).Returns(ConstantTest.ConnectionId);
        _mockClients.Setup(clients => clients.All).Returns(_mockDuckCityClient.Object);
        _mockClients.Setup(clients => clients.Group(ConstantTest.Code)).Returns(_mockDuckCityClient.Object);
    }

    /**
    * Tests
    */
    [Theory]
    [InlineData(ConstantTest.UserId, ConstantTest.UserName, ConstantTest.Code, ConstantTest.ConnectionId)]
    public async Task JoinRoomAsyncTest(string userId, string userName, string roomCode, string connectionId)
    {
        // Given
        Room room = new("", userId, userName, "",
            ConstantTest.True, ConstantTest.Five, connectionId, ConstantTest.Code);

        //Mock
        _mockRoomService.Setup(service => service.JoinRoom(ConstantTest.ConnectionId, userId, userName, roomCode)).Returns(room);

        //When
        await _duckCityHub.JoinRoomAsync(roomCode, userId, userName);
        
        //Verify
        _mockRoomService.Verify(service => service.JoinRoom(ConstantTest.ConnectionId, userId, userName, roomCode), Times.Once);
        _mockGroups.Verify(groups => groups.AddToGroupAsync(ConstantTest.ConnectionId, roomCode, CancellationToken.None), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.Code, ConstantTest.UserId, ConstantTest.ConnectionId)]
    public async Task LeaveRoomAsyncWithoutResponseTest(string roomCode, string userId, string connectionId)
    {
        //When
        await _duckCityHub.LeaveRoomAsync(roomCode, userId);
        
        //Verify
        _mockRoomService.Verify(service => service.LeaveRoom(roomCode, connectionId), Times.Once);
        _mockGroups.Verify(groups => groups.RemoveFromGroupAsync(ConstantTest.ConnectionId, roomCode, CancellationToken.None), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Never);
    }
    
    [Theory]
    [InlineData(ConstantTest.Code, ConstantTest.UserId, ConstantTest.ConnectionId)]
    public async Task LeaveRoomAndDisconnectAsyncWithResponseTest(string roomCode, string userId, string connectionId)
    {
        // Given
        Room room = new("", userId, "", "",
            ConstantTest.True, ConstantTest.Five, connectionId, ConstantTest.Code);

        // Mock
        _mockRoomService.Setup(service => service.LeaveRoom(roomCode, connectionId)).Returns(room);

        //When
        await _duckCityHub.LeaveRoomAsync(roomCode, userId);
        
        //Verify
        _mockRoomService.Verify(service => service.LeaveRoom(roomCode, connectionId), Times.Once);
        _mockGroups.Verify(groups => groups.RemoveFromGroupAsync(ConstantTest.ConnectionId, roomCode, CancellationToken.None), Times.Once);

        _mockRoomPreviewService.Verify(r => r.UpdateRoomPreview(It.IsAny<RoomPreview>()), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Once);

        _mockRoomPreviewService.Verify(r => r.DeleteRoomPreview(roomCode), Times.Never);
    }
    
    [Theory]
    [InlineData(ConstantTest.Code, ConstantTest.UserId, ConstantTest.ConnectionId)]
    public async Task PlayerReadyAsyncTest(string roomCode, string userId, string connectionId)
    {
        //Given
        Room room = new("", userId, "", "", ConstantTest.True, ConstantTest.Five, connectionId, ConstantTest.Code);

        //Mock
        _mockRoomService.Setup(service => service.SetPlayerReady(roomCode, connectionId)).Returns(room);

        //When
        await _duckCityHub.PlayerReadyAsync(roomCode);
        
        //Verify
        _mockRoomService.Verify(service => service.SetPlayerReady(roomCode, connectionId), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Once);
    }
    
    [Fact]
    public async Task OnConnectedAsyncTest()
    {
        //When
        await _duckCityHub.OnConnectedAsync();
    }
    
    [Theory]
    [InlineData(ConstantTest.ConnectionId)]
    public async Task OnDisconnectedAsyncTest(string connectionId)
    {
        //Given
        const Exception? exception = null;
        Room room = new("", "", "", "", ConstantTest.True, ConstantTest.Five, connectionId, ConstantTest.Code);

        //Mock
        _mockRoomService.Setup(service => service.DisconnectFromRoom(connectionId)).Returns(room);

        //When
        await _duckCityHub.OnDisconnectedAsync(exception);
        
        //Verify
        _mockRoomService.Verify(service => service.DisconnectFromRoom(connectionId), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Once);
    }
    
    [Theory]
    [InlineData(ConstantTest.ConnectionId)]
    public async Task OnDisconnectedAsyncWithoutResponseTest(string connectionId)
    {
        //Given
        const Exception? exception = null;
        
        //When 
        await _duckCityHub.OnDisconnectedAsync(exception);
        
        //Verify
        _mockRoomService.Verify(service => service.DisconnectFromRoom(connectionId), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Never);
    }
}