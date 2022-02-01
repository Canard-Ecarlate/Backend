using System.Collections.Generic;
using DuckCity.Application.Services;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.Repositories.Interfaces;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Application
{
    public class RoomServiceUt
    {
        // Class to test
        private readonly RoomService _roomService;
        
        // Mock
        private readonly Mock<IUserRepository> _mockUserRep = new();
        private readonly Mock<IRoomRepository> _mockRoomRep = new();

        // Constructor
        public RoomServiceUt()
        {
            _roomService = new RoomService(_mockUserRep.Object, _mockRoomRep.Object);
        }

        /**
         * Tests
         */
        [Fact]
        public void FindAllRoomsTest()
        {
            _mockRoomRep.Setup(mock => mock.FindAllRooms()).Returns(new List<Room>());

            IEnumerable<Room> result = _roomService.FindAllRooms();
            Assert.Empty(result);
            _mockRoomRep.Verify(mock => mock.FindAllRooms(), Times.Once);
        }

        [Theory]
        [InlineData("something not ObjectId")]
        [InlineData(ConstantTest.ObjectId1)]
        public void FindRoomTest(string roomId)
        {
            _mockRoomRep.Setup(mock => mock.FindById(roomId)).Returns(new Room(roomId, "", "", true, 5));

            try
            {
                Room? result = _roomService.FindRoom(roomId);
                Assert.NotNull(result);
                _mockRoomRep.Verify(mock => mock.FindById(roomId), Times.Once);
            }
            catch (IdNotValidException e)
            {
                Assert.True(!ObjectId.TryParse(roomId, out _));
                Assert.NotNull(e);
            }
        }

        [Theory]
        [InlineData(ConstantTest.String, ConstantTest.ObjectId1, ConstantTest.String, ConstantTest.True,
            ConstantTest.Five, 1)]
        [InlineData(ConstantTest.String, ConstantTest.String, ConstantTest.String, ConstantTest.True, ConstantTest.Five,
            1)]
        [InlineData(ConstantTest.String, ConstantTest.ObjectId1, ConstantTest.String, ConstantTest.True,
            ConstantTest.Five, 0)]
        [InlineData("", ConstantTest.ObjectId1, ConstantTest.String, ConstantTest.True, ConstantTest.Five, 1)]
        public void AddRoomsTest(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers,
            int countUser)
        {
            _mockUserRep.Setup(mock => mock.CountUserById(hostId)).Returns(countUser);

            try
            {
                Room roomResult = _roomService.AddRooms(roomName, hostId, hostName, isPrivate, nbPlayers);
                Assert.NotNull(roomResult);
                Assert.NotNull(roomResult.RoomConfiguration);
                Assert.Equal(roomName, roomResult.Name);
                Assert.Equal(hostId, roomResult.HostId);
                Assert.Equal(hostName, roomResult.HostName);
                Assert.Equal(isPrivate, roomResult.RoomConfiguration?.IsPrivate);
                Assert.Equal(nbPlayers, roomResult.RoomConfiguration?.NbPlayers);
                _mockRoomRep.Verify(mock => mock.Create(It.IsAny<Room>()), Times.Once);
            }
            catch (IdNotValidException e)
            {
                Assert.True(!ObjectId.TryParse(hostId, out _));
                Assert.NotNull(e);
            }
            catch (RoomNameNullException e)
            {
                Assert.True(string.IsNullOrEmpty(roomName));
                Assert.NotNull(e);
            }
            catch (HostIdNoExistException e)
            {
                Assert.True(_mockUserRep.Object.CountUserById(hostId) == 0);
                Assert.NotNull(e);
            }
        }
    }
}