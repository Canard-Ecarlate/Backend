using DuckCity.Application.Validations;
using DuckCity.Domain;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.GameContainerRepository;
using DuckCity.Infrastructure.RoomPreviewRepository;
using DuckCity.Infrastructure.UserRepository;

namespace DuckCity.Application.ContainerGameApiService;

public class GameContainerService : IGameContainerService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomPreviewRepository _roomPreviewRepository;
    private readonly IGameContainerRepository _gameContainerRepository;

    private const int MaxNbRooms = 10000;

    public GameContainerService(IUserRepository userRepository, IRoomPreviewRepository roomPreviewRepository,
        IGameContainerRepository gameContainerRepository)
    {
        _userRepository = userRepository;
        _roomPreviewRepository = roomPreviewRepository;
        _gameContainerRepository = gameContainerRepository;
    }

    public GameContainer ContainerAccessToCreateRoom(string roomName, string hostId)
    {
        CheckValid.CreateRoom(_roomPreviewRepository, _userRepository, roomName, hostId);

        List<GameContainer> gameContainersNotFull = _gameContainerRepository.FindAll().Where(gameContainer =>
            _roomPreviewRepository.CountByGameContainerId(gameContainer.Id) < MaxNbRooms).ToList();

        if (gameContainersNotFull.Count >= 1)
        {
            return gameContainersNotFull[0];
        }

        GameContainer newGameContainer = new();
        _gameContainerRepository.Create(newGameContainer);

        return newGameContainer;
    }

    public GameContainer ContainerAccessToJoinRoom(string roomId, string userId)
    {
        CheckValid.JoinRoom(_roomPreviewRepository, _userRepository, userId);
        RoomPreview roomPreview = _roomPreviewRepository.FindById(roomId);
        GameContainer gameContainer = _gameContainerRepository.FindById(roomPreview.ContainerId);
        return gameContainer;
    }
}