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
    public void CreateRoomTest(string connectionId, string userId, string userName, string roomId)
    {
        // Given
        Room room = new("", userId, userName, "",
            ConstantTest.True, ConstantTest.Five, connectionId,ConstantTest.Code)
        {
            Id = roomId
        };

        // When
        _roomService.CreateRoom(room);
        
        // Verify
        _mockRoomRep.Verify(r => r.Create(room), Times.Once);
    }
    [Theory]
    [InlineData(ConstantTest.ConnectionId, ConstantTest.UserId, ConstantTest.UserName, ConstantTest.RoomId)]
    public void JoinRoomAndReConnectTest(string connectionId, string userId, string userName, string roomCode)
    {
        // Given
        Room room = new("", ConstantTest.UserId, ConstantTest.UserName, "",
            ConstantTest.True, ConstantTest.Five, connectionId,ConstantTest.Code)
        {
            Id = roomCode
        };

        // Mock
        _mockRoomRep.Setup(r => r.FindByCode(roomCode)).Returns(room);

        // When
        Room roomResult = _roomService.JoinRoom(connectionId, userId, userName, roomCode);
        
        // Then
        Assert.NotNull(roomResult);
        Assert.Single(roomResult.Players);
        Player player = roomResult.Players.Single();
        Assert.Equal(connectionId, player.ConnectionId);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindByCode(roomCode), Times.Once);
        _mockRoomRep.Verify(r => r.Update(roomResult), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.ConnectionId, ConstantTest.UserId, ConstantTest.UserName, ConstantTest.Code)]
    public void JoinRoomNewPlayerTest(string connectionId, string userId, string userName, string roomCode)
    {
        // Given
        Room room = new("", ConstantTest.UserId2, ConstantTest.UserName2, "",
            ConstantTest.True, ConstantTest.Five, connectionId,ConstantTest.Code)
        {
            Id = roomCode
        };
        // Mock
        _mockRoomRep.Setup(r => r.FindByCode(roomCode)).Returns(room);

        // When
        Room roomResult = _roomService.JoinRoom(connectionId, userId, userName, roomCode);

        // Then
        Assert.NotNull(roomResult);
        Assert.Equal(ConstantTest.Two, roomResult.Players.Count);
        Player player = roomResult.Players.First(p => p.Id == userId);
        Assert.Equal(connectionId, player.ConnectionId);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindByCode(roomCode), Times.Once);
        _mockRoomRep.Verify(r => r.Update(roomResult), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.ConnectionId)]
    public void LeaveRoomAndDestroyRoomTest(string roomId, string connectionId)
    {
        // Given
        Room room = new("", ConstantTest.UserId, ConstantTest.UserName, "",
            ConstantTest.True, ConstantTest.Five, connectionId,ConstantTest.Code)
        {
            Id = roomId
        };

        // Mock
        _mockRoomRep.Setup(r => r.FindById(roomId)).Returns(room);

        // When
        Room? roomResult = _roomService.LeaveRoom(roomId, connectionId);
        
        // Then
        Assert.Null(roomResult);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindById(roomId), Times.Once);
        _mockRoomRep.Verify(r => r.Delete(room), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.ConnectionId)]
    public void LeaveRoomAndKeepRoomTest(string roomId, string connectionId)
    {
        // Given
        Room room = new("", ConstantTest.UserId, ConstantTest.UserName, "",
            ConstantTest.True, ConstantTest.Five, connectionId,ConstantTest.Code)
        {
            Id = roomId
        };

        Player player = new(ConstantTest.ConnectionId2, ConstantTest.UserId2, ConstantTest.UserName2);
        room.Players.Add(player);
        
        // Mock
        _mockRoomRep.Setup(r => r.FindById(roomId)).Returns(room);

        // When
        Room? roomResult = _roomService.LeaveRoom(roomId, connectionId);
        
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
    public void DisconnectFromRoomNotFoundTest(string connectionId)
    {
        // When
        Room? roomResult = _roomService.DisconnectFromRoom(connectionId);
        
        // Then
        Assert.Null(roomResult);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindByConnectionId(connectionId), Times.Once);
        _mockRoomRep.Verify(r => r.Update(It.IsAny<Room>()), Times.Never);
    }
    
    [Theory]
    [InlineData(ConstantTest.ConnectionId)]
    public void DisconnectFromRoomTest(string connectionId)
    {
        // Given
        Room room = new("", ConstantTest.UserId, ConstantTest.UserName, "",
            ConstantTest.True, ConstantTest.Five, connectionId,ConstantTest.Code)
        {
            Id = ConstantTest.RoomId
        };

        // Mock
        _mockRoomRep.Setup(r => r.FindByConnectionId(connectionId)).Returns(room);
        
        // When
        Room? roomResult = _roomService.DisconnectFromRoom(connectionId);
        
        // Then
        Assert.NotNull(roomResult);
        
        // Verify
        _mockRoomRep.Verify(r => r.FindByConnectionId(connectionId), Times.Once);
        _mockRoomRep.Verify(r => r.Update(It.IsAny<Room>()), Times.Once);
    }

    [Theory]
    [InlineData(ConstantTest.RoomId, ConstantTest.ConnectionId)]
    public void SetPlayerReadyTest(string roomId, string connectionId)
    {
        // Given
        Room room = new("", ConstantTest.UserId, ConstantTest.UserName, "",
            ConstantTest.True, ConstantTest.Five, connectionId,ConstantTest.Code)
        {
            Id = roomId
        };
        
        // Mock
        _mockRoomRep.Setup(r => r.FindById(roomId)).Returns(room);

        // When
        Room roomResult = _roomService.SetPlayerReady(roomId, connectionId);
        
        // Then
        Assert.NotNull(roomResult);
        
        // Verify
        _mockRoomRep.Verify(r => r.Update(It.IsAny<Room>()), Times.Once);
    }

}