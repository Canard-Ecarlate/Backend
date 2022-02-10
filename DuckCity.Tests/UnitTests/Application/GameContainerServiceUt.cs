using DuckCity.Application.ContainerGameApiService;
using DuckCity.Infrastructure.GameContainerRepository;
using DuckCity.Infrastructure.RoomPreviewRepository;
using DuckCity.Infrastructure.UserRepository;
using Moq;

namespace DuckCity.Tests.UnitTests.Application;

public class GameContainerServiceUt
{
    // Class to test
    private readonly GameContainerService _gameContainerService;

    // Mock
    private readonly Mock<IUserRepository> _mockUserRep = new();
    private readonly Mock<IRoomPreviewRepository> _mockRoomPreviewRep = new();
    private readonly Mock<IGameContainerRepository> _mockGameContainerRep = new();

    // Constructor
    public GameContainerServiceUt()
    {
        _gameContainerService = new GameContainerService(_mockUserRep.Object, _mockRoomPreviewRep.Object, _mockGameContainerRep.Object);
    }
}