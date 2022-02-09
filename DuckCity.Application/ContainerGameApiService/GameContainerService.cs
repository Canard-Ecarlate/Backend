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

    public GameContainerService(IUserRepository userRepository, IRoomPreviewRepository roomPreviewRepository, IGameContainerRepository gameContainerRepository)
    {
        _userRepository = userRepository;
        _roomPreviewRepository = roomPreviewRepository;
        _gameContainerRepository = gameContainerRepository;
    }

    public GameContainer ContainerAccessToCreateRoom(string roomName, string hostId)
    {
        CheckValid.CreateRoom(_roomPreviewRepository, _userRepository, roomName, hostId);
        GameContainer gameContainer = _gameContainerRepository.FindLastOne();
        return gameContainer;
    }
    
    public GameContainer ContainerAccessToJoinRoom(string roomId, string userId)
    {
        CheckValid.JoinRoom(_roomPreviewRepository, _userRepository, userId);
        RoomPreview roomPreview = _roomPreviewRepository.FindById(roomId);
        GameContainer gameContainer = _gameContainerRepository.FindById(roomPreview.ContainerId);
        return gameContainer;
    }

    public void IncrementContainerNbRooms()
    {
        GameContainer gameContainer = _gameContainerRepository.FindById("62027037376c84d6914f9344");
        gameContainer.NbRooms++;
        _gameContainerRepository.Update(gameContainer);
    }

    public void DecrementContainerNbRooms()
    {
        GameContainer gameContainer = _gameContainerRepository.FindById("62027037376c84d6914f9344");
        gameContainer.NbRooms--;
        _gameContainerRepository.Update(gameContainer);
    }
}