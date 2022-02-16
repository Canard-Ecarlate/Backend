using System.Collections.Generic;
using DuckCity.Application.RoomPreviewService;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.RoomPreviewRepository;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Application
{
    public class RoomPreviewServiceUt
    {
        // Class to test
        private readonly RoomPreviewService _roomPreviewService;

        // Mock
        private readonly Mock<IRoomPreviewRepository> _mockRoomRep = new();

        // Constructor
        public RoomPreviewServiceUt()
        {
            _roomPreviewService = new RoomPreviewService(_mockRoomRep.Object);
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
            _mockRoomRep.Setup(mock => mock.FindById(roomId))
                .Returns(new RoomPreview(new Room("", "", "", "", ConstantTest.True, ConstantTest.Five, "")));

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
            }
        }
    }
}