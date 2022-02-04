using System.Collections.Generic;
using DuckCity.Api.Controllers;
using DuckCity.Api.DTO.Room;
using DuckCity.Application.Services.Interfaces;
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
        private readonly RoomController _roomController;

        // Mock
        private readonly Mock<IRoomService> _mockRoomService = new();

        // Constructor
        public RoomControllerUt()
        {
            _roomController = new RoomController(_mockRoomService.Object)
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
            IEnumerable<Room> rooms = new List<Room> {new(roomName, "","",true,5)};

            // Mock
            _mockRoomService.Setup(mock => mock.FindAllRooms()).Returns(rooms);

            // When
            OkObjectResult? allRooms = _roomController.FindAllRooms().Result as OkObjectResult;

            // Then
            Assert.NotNull(allRooms);
            Assert.True(allRooms?.Value is IEnumerable<Room>);
            List<Room> roomsResult = (List<Room>) allRooms?.Value!;
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
            Room room = new(roomName, "", "", isPrivate, nbPlayers);

            // Mock
            _mockRoomService.Setup(mock => mock.CreateAndJoinRoom(roomName, hostId, hostName, isPrivate, nbPlayers))
                .Returns(room);

            // When
            OkObjectResult? roomCreated = _roomController.CreateAndJoinRoom(creationDto).Result as OkObjectResult;

            // Then
            Assert.NotNull(roomCreated);
            Assert.True(roomCreated?.Value is Room);
            Room roomResult = (Room) roomCreated?.Value!;
            Assert.Equal(room, roomResult);
        }

        [Theory]
        [InlineData(ConstantTest.UserId, ConstantTest.String, ConstantTest.UserId)]
        public void JoinRoomTest(string userId, string userName, string roomId)
        {
            // Given
            UserAndRoomDto userAndRoomDto = new()
                {UserId = userId, UserName = userName, RoomId = roomId};
            Room room = new(roomId, "", "", true, 5);

            // Mock
            _mockRoomService.Setup(mock => mock.JoinRoom(roomId, userId, userName)).Returns(room);

            // When
            OkObjectResult? roomJoined = _roomController.JoinRoom(userAndRoomDto).Result as OkObjectResult;

            // Then
            Assert.NotNull(roomJoined);
            Assert.True(roomJoined?.Value is Room);
            Room roomResult = (Room) roomJoined?.Value!;
            Assert.Equal(room, roomResult);
        }
    }
}