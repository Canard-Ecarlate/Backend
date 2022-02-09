using System.Collections.Generic;
using DuckCity.Application.RoomPreviewService;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.RoomPreviewRepository;
using DuckCity.Infrastructure.UserRepository;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Application
{
    public class RoomServiceUt
    {
        // Class to test
        private readonly RoomPreviewService _roomPreviewService;
        
        // Mock
        private readonly Mock<IUserRepository> _mockUserRep = new();
        private readonly Mock<IRoomPreviewRepository> _mockRoomRep = new();

        // Constructor
        public RoomServiceUt()
        {
            _roomPreviewService = new RoomPreviewService(_mockRoomRep.Object, _mockUserRep.Object);
        }

        /**
         * Tests
         */
        [Fact]
        public void FindAllRoomsTest()
        {
            _mockRoomRep.Setup(mock => mock.FindAllRooms()).Returns(new List<RoomPreview>());

            IEnumerable<RoomPreview> result = _roomPreviewService.FindAllRooms();
            Assert.Empty(result);
            _mockRoomRep.Verify(mock => mock.FindAllRooms(), Times.Once);
        }

        [Theory]
        [InlineData("something not ObjectId")]
        [InlineData(ConstantTest.UserId)]
        public void FindRoomTest(string roomId)
        {
            _mockRoomRep.Setup(mock => mock.FindById(roomId)).Returns(new RoomPreview(roomId, "", "", ConstantTest.True, ConstantTest.Five));

            try
            {
                RoomPreview result = _roomPreviewService.FindRoom(roomId);
                Assert.NotNull(result);
                _mockRoomRep.Verify(mock => mock.FindById(roomId), Times.Once);
            }
            catch (IdNotValidException e)
            {
                Assert.True(!ObjectId.TryParse(roomId, out _));
                Assert.NotNull(e);
                _mockRoomRep.Verify(mock => mock.FindById(roomId), Times.Never);
            }
        }

        [Theory]
        [InlineData(ConstantTest.String, ConstantTest.UserId, ConstantTest.String, ConstantTest.True,
            ConstantTest.Five, 1)]
        [InlineData(ConstantTest.String, ConstantTest.String, ConstantTest.String, ConstantTest.True, ConstantTest.Five,
            1)]
        [InlineData(ConstantTest.String, ConstantTest.UserId, ConstantTest.String, ConstantTest.True,
            ConstantTest.Five, 0)]
        [InlineData("", ConstantTest.UserId, ConstantTest.String, ConstantTest.True, ConstantTest.Five, 1)]
        public void AddRoomsTest(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers,
            int countUser)
        {
            _mockUserRep.Setup(mock => mock.CountUserById(hostId)).Returns(countUser);

            try
            {
                RoomPreview roomPreviewResult = _roomPreviewService.CreateAndJoinRoomPreview(roomName, hostId, hostName, isPrivate, nbPlayers);
                Assert.NotNull(roomPreviewResult);
                Assert.NotNull(roomPreviewResult.RoomConfiguration);
                Assert.Equal(roomName, roomPreviewResult.Name);
                Assert.Equal(hostId, roomPreviewResult.HostId);
                Assert.Equal(hostName, roomPreviewResult.HostName);
                Assert.Equal(isPrivate, roomPreviewResult.RoomConfiguration.IsPrivate);
                Assert.Equal(nbPlayers, roomPreviewResult.RoomConfiguration.NbPlayers);
                _mockRoomRep.Verify(mock => mock.Create(It.IsAny<RoomPreview>()), Times.Once);
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