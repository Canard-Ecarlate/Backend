using System.Collections.Generic;
using DuckCity.Api.Controllers;
using DuckCity.Api.DTO.Room;
using DuckCity.Application.RoomPreviewService;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Api
{
    public class RoomControllerUt
    {
        // Class to test
        private readonly RoomPreviewController _roomPreviewController;

        // Mock
        private readonly Mock<IRoomPreviewService> _mockRoomPreviewService = new();

        // Constructor
        public RoomControllerUt()
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
            IEnumerable<RoomPreview> rooms = new List<RoomPreview> {new(roomName, "","",true,5)};

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

        [Theory]
        [InlineData(ConstantTest.String, ConstantTest.UserId, ConstantTest.String, ConstantTest.True,
            ConstantTest.Five)]
        public void CreateRoomTest(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers)
        {
            // Given
            RoomCreationDto creationDto = new()
                {HostId = hostId, Name = roomName, HostName = hostName, IsPrivate = isPrivate, NbPlayers = nbPlayers};
            RoomPreview roomPreview = new(roomName, "", "", isPrivate, nbPlayers);

            // Mock
            _mockRoomPreviewService.Setup(mock => mock.CreateAndJoinRoomPreview(roomName, hostId, hostName, isPrivate, nbPlayers))
                .Returns(roomPreview);

            // When
            OkObjectResult? roomCreated = _roomPreviewController.CreateAndJoinRoom(creationDto).Result as OkObjectResult;

            // Then
            Assert.NotNull(roomCreated);
            Assert.True(roomCreated?.Value is RoomPreview);
            RoomPreview roomPreviewResult = (RoomPreview) roomCreated?.Value!;
            Assert.Equal(roomPreview, roomPreviewResult);
        }

        [Theory]
        [InlineData(ConstantTest.UserId, ConstantTest.String, ConstantTest.UserId)]
        public void JoinRoomTest(string userId, string userName, string roomId)
        {
            // Given
            UserAndRoomDto userAndRoomDto = new()
                {UserId = userId, UserName = userName, RoomId = roomId};
            RoomPreview roomPreview = new(roomId, "", "", true, 5);

            // Mock
            _mockRoomPreviewService.Setup(mock => mock.JoinRoomPreview(roomId, userId)).Returns(roomPreview);

            // When
            OkObjectResult? roomJoined = _roomPreviewController.JoinRoom(userAndRoomDto).Result as OkObjectResult;

            // Then
            Assert.NotNull(roomJoined);
            Assert.True(roomJoined?.Value is RoomPreview);
            RoomPreview roomPreviewResult = (RoomPreview) roomJoined?.Value!;
            Assert.Equal(roomPreview, roomPreviewResult);
        }
    }
}