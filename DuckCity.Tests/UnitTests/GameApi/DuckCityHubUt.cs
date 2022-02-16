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
        _mockClients.Setup(clients => clients.Group(ConstantTest.RoomId)).Returns(_mockDuckCityClient.Object);
    }

    /**
    * Tests
    */
    [Theory]
    [InlineData(ConstantTest.UserId, ConstantTest.UserName, ConstantTest.RoomId, ConstantTest.ConnectionId)]
    public async Task JoinRoomAndConnectAsyncTest(string userId, string userName, string roomId, string connectionId)
    {
        // Given
        Room room = new(roomId, connectionId, userId, userName);
        
        //Mock
        _mockRoomService.Setup(service => service.JoinRoomAndConnect(ConstantTest.ConnectionId, userId, userName, roomId)).Returns(room);

        //When
        await _duckCityHub.JoinRoomAndConnectAsync(roomId, userId, userName);
        
        //Verify
        _mockRoomService.Verify(service => service.JoinRoomAndConnect(ConstantTest.ConnectionId, userId, userName, roomId), Times.Once);
        _mockGroups.Verify(groups => groups.AddToGroupAsync(ConstantTest.ConnectionId, roomId, CancellationToken.None), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.UserId, ConstantTest.ConnectionId)]
    public async Task LeaveRoomAndDisconnectAsyncWithoutResponseTest(string roomId, string userId, string connectionId)
    {
        //When
        await _duckCityHub.LeaveRoomAndDisconnectAsync(roomId, userId);
        
        //Verify
        _mockRoomPreviewService.Verify(service => service.LeaveRoomPreview(roomId, userId), Times.Once());
        _mockRoomService.Verify(service => service.LeaveRoomAndDisconnect(roomId, connectionId), Times.Once);
        _mockGroups.Verify(groups => groups.RemoveFromGroupAsync(ConstantTest.ConnectionId, roomId, CancellationToken.None), Times.Never);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Never);
    }
    
    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.UserId, ConstantTest.ConnectionId)]
    public async Task LeaveRoomAndDisconnectAsyncWithResponseTest(string roomId, string userId, string connectionId)
    {
        // Given
        RoomPreview roomPreview = new("", userId, "", ConstantTest.True, ConstantTest.Five)
        {
            Id = roomId
        };
        Room room = new(roomId, connectionId, userId, "");
        
        // Mock
        _mockRoomPreviewService.Setup(service => service.LeaveRoomPreview(roomId, userId)).Returns(roomPreview);
        _mockRoomService.Setup(service => service.LeaveRoomAndDisconnect(roomId, connectionId)).Returns(room);

        //When
        await _duckCityHub.LeaveRoomAndDisconnectAsync(roomId, userId);
        
        //Verify
        _mockRoomPreviewService.Verify(service => service.LeaveRoomPreview(roomId, userId), Times.Once());
        _mockRoomService.Verify(service => service.LeaveRoomAndDisconnect(roomId, connectionId), Times.Once);
        _mockGroups.Verify(groups => groups.RemoveFromGroupAsync(ConstantTest.ConnectionId, roomId, CancellationToken.None), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Once);
    }
    
    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.UserId, ConstantTest.ConnectionId)]
    public async Task PlayerReadyAsyncTest(string roomId, string userId, string connectionId)
    {
        //Given
        Room room = new(roomId, connectionId, userId, "");

        //Mock
        _mockRoomService.Setup(service => service.SetPlayerReady(roomId, connectionId)).Returns(room);

        //When
        await _duckCityHub.PlayerReadyAsync(roomId);
        
        //Verify
        _mockRoomService.Verify(service => service.SetPlayerReady(roomId, connectionId), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Once);
    }
    
    [Fact]
    public async Task OnConnectedAsyncTest()
    {
        //When
        await _duckCityHub.OnConnectedAsync();
    }
    
    [Theory]
    [InlineData(ConstantTest.ConnectionId, ConstantTest.RoomId)]
    public async Task OnDisconnectedAsyncTest(string connectionId, string roomId)
    {
        //Given
        Exception? exception = null;
        Room room = new(roomId, connectionId, "", "");

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
        Exception? exception = null;
        
        //When
        await _duckCityHub.OnDisconnectedAsync(exception);
        
        //Verify
        _mockRoomService.Verify(service => service.DisconnectFromRoom(connectionId), Times.Once);
        _mockDuckCityClient.Verify(clients => clients.PushPlayers(It.IsAny<IEnumerable<PlayerInWaitingRoomDto>>()), Times.Never);
    }
}