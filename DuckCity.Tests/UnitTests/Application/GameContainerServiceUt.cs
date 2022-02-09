namespace DuckCity.Tests.UnitTests.Application;

public class GameContainerServiceUt
{
    /*[Theory]
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
            RoomPreview roomPreviewResult =
                _roomPreviewService.AccessToCreateRoom(roomName, hostId, hostName, isPrivate, nbPlayers);
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
    }*/
}