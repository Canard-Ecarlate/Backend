using System.Linq;
using DuckCity.Application.RoomService;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.RoomRepository;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Application;

public class RoomServiceUt
{
    // Class to test
    private readonly RoomService _roomService;
        
    // Mock
    private readonly Mock<IRoomRepository> _mockRoomRep = new();

    // Constructor
    public RoomServiceUt()
    {
        _roomService = new RoomService(_mockRoomRep.Object);
    }

    /**
     * Tests
     */
    [Theory]
    [InlineData(ConstantTest.ConnectionId, ConstantTest.UserId, ConstantTest.UserName, ConstantTest.RoomId)]
    public void CreateJoinRoomAndConnectTest(string connectionId, string userId, string userName, string roomId)
    {
        // When
        Room roomResult = _roomService.JoinRoomAndConnect(connectionId, userId, userName, roomId);
        
        // Assert
        Assert.NotNull(roomResult);
        Assert.Single(roomResult.Players);
        Player player = roomResult.Players.Single();
        Assert.Equal(userId, player.Id);
        Assert.Equal(userName, player.Name);
        Assert.Equal(connectionId, player.ConnectionId);
        Assert.Equal(roomId, roomResult.RoomId);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindById(roomId), Times.Once);
        _mockRoomRep.Verify(r => r.Create(roomResult), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.ConnectionId, ConstantTest.UserId, ConstantTest.UserName, ConstantTest.RoomId)]
    public void JoinRoomAndReConnectTest(string connectionId, string userId, string userName, string roomId)
    {
        // Given
        Room room = new(roomId, "", ConstantTest.UserId, ConstantTest.UserName);

        // Mock
        _mockRoomRep.Setup(r => r.FindById(roomId)).Returns(room);

        // When
        Room roomResult = _roomService.JoinRoomAndConnect(connectionId, userId, userName, roomId);
        
        // Then
        Assert.NotNull(roomResult);
        Assert.Single(roomResult.Players);
        Player player = roomResult.Players.Single();
        Assert.Equal(connectionId, player.ConnectionId);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindById(roomId), Times.Once);
        _mockRoomRep.Verify(r => r.Update(roomResult), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.ConnectionId, ConstantTest.UserId, ConstantTest.UserName, ConstantTest.RoomId)]
    public void JoinRoomAndConnectNewPlayerTest(string connectionId, string userId, string userName, string roomId)
    {
        // Given
        Room room = new(roomId, "", ConstantTest.UserId2, ConstantTest.UserName2);

        // Mock
        _mockRoomRep.Setup(r => r.FindById(roomId)).Returns(room);

        // When
        Room roomResult = _roomService.JoinRoomAndConnect(connectionId, userId, userName, roomId);

        // Then
        Assert.NotNull(roomResult);
        Assert.Equal(2, roomResult.Players.Count);
        Player player = roomResult.Players.First(p => p.Id == userId);
        Assert.Equal(connectionId, player.ConnectionId);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindById(roomId), Times.Once);
        _mockRoomRep.Verify(r => r.Update(roomResult), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.ConnectionId)]
    public void LeaveRoomAndDisconnectAndDestroyRoomTest(string roomId, string connectionId)
    {
        // Given
        Room room = new(roomId, connectionId, ConstantTest.UserId, ConstantTest.UserName);

        // Mock
        _mockRoomRep.Setup(r => r.FindById(roomId)).Returns(room);

        // When
        Room? roomResult = _roomService.LeaveRoomAndDisconnect(roomId, connectionId);
        
        // Then
        Assert.Null(roomResult);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindById(roomId), Times.Once);
        _mockRoomRep.Verify(r => r.Delete(room), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.ConnectionId)]
    public void LeaveRoomAndDisconnectAndKeepRoomTest(string roomId, string connectionId)
    {
        // Given
        Room room = new(roomId, connectionId, ConstantTest.UserId, ConstantTest.UserName);
        Player player = new(ConstantTest.ConnectionId2, ConstantTest.UserId2, ConstantTest.UserName2);
        room.Players.Add(player);
        
        // Mock
        _mockRoomRep.Setup(r => r.FindById(roomId)).Returns(room);

        // When
        Room? roomResult = _roomService.LeaveRoomAndDisconnect(roomId, connectionId);
        
        // Then
        Assert.NotNull(roomResult);
        Assert.Single(roomResult!.Players);
        Assert.Null(roomResult.Players.SingleOrDefault(p => p.ConnectionId == connectionId));
        
        // Verify
        _mockRoomRep.Verify(r => r.FindById(roomId), Times.Once);
        _mockRoomRep.Verify(r => r.Update(room), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.ConnectionId)]
    public void DisconnectFromRoomTest(string connectionId)
    {
        
    }

    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.ConnectionId)]
    public void SetPlayerReadyTest(string roomId, string connectionId)
    {
        
    }

}