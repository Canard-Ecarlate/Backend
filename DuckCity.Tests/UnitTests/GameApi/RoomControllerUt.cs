using DuckCity.Application.Services.Interfaces;
using DuckCity.GameApi.Controllers;
using DuckCity.GameApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.GameApi;

public class RoomControllerUt
{
    // Class to test
    private readonly WaitingRoomController _waitingRoomController;

    // Mock
    private readonly Mock<IRoomService> _mockRoomService = new();

    // Constructor
    public RoomControllerUt()
    {
        _waitingRoomController = new WaitingRoomController(_mockRoomService.Object)
        {
            ControllerContext = {HttpContext = new Mock<HttpContext>().Object}
        };
    }

    /**
     * Tests
     */
    [Theory]
    [InlineData(ConstantTest.ObjectId, ConstantTest.ObjectId, ConstantTest.True)]
    [InlineData(ConstantTest.ObjectId, ConstantTest.ObjectId, ConstantTest.False)]
    public void LeaveRoomTest(string userId, string roomId, bool leaveResponse)
    {
        // Given
        UserIdAndRoomIdDto userIdAndRoomIdDto = new() {UserId = userId, RoomId = roomId};
        
        // Mock
        _mockRoomService.Setup(mock => mock.LeaveRoom(roomId, userId)).Returns(leaveResponse);

        // When
        OkObjectResult? leftRoom = _waitingRoomController.LeaveRoom(userIdAndRoomIdDto).Result as OkObjectResult;

        // Then
        Assert.NotNull(leftRoom);
        Assert.True(leftRoom?.Value is bool);
        bool leftRoomResult = (bool) leftRoom?.Value!;
        Assert.Equal(leaveResponse, leftRoomResult);
    }
}