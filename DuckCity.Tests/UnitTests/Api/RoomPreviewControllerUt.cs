using System.Collections.Generic;
using DuckCity.Api.Controllers;
using DuckCity.Application.RoomPreviewService;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Api;

public class RoomPreviewControllerUt
{
    // Class to test
    private readonly RoomPreviewController _roomPreviewController;

    // Mock
    private readonly Mock<IRoomPreviewService> _mockRoomPreviewService = new();

    // Constructor
    public RoomPreviewControllerUt()
    {
        _roomPreviewController = new RoomPreviewController(_mockRoomPreviewService.Object)
        {
            ControllerContext = {HttpContext = new Mock<HttpContext>().Object}
        };
    }

    /**
     * Tests
     */
    [Theory]
    [InlineData(ConstantTest.String)]
    public void FindAllRoomsTest(string roomName)
    {
        // Given
        IEnumerable<RoomPreview> rooms = new List<RoomPreview> {new(new Room(roomName,"","","", true, 5, ""))};

        // Mock
        _mockRoomPreviewService.Setup(mock => mock.FindAllRooms()).Returns(rooms);

        // When
        OkObjectResult? allRooms = _roomPreviewController.FindAllRooms().Result as OkObjectResult;

        // Then
        Assert.NotNull(allRooms);
        Assert.True(allRooms?.Value is IEnumerable<RoomPreview>);
        List<RoomPreview> roomsResult = (List<RoomPreview>) allRooms?.Value!;
        Assert.Single(roomsResult);
        Assert.Equal(rooms, roomsResult);
    }
}