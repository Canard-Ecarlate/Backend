using DuckCity.Application.Validations;
using DuckCity.Domain;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.GameContainerRepository;
using DuckCity.Infrastructure.RoomPreviewRepository;
using DuckCity.Infrastructure.UserRepository;
using System.Diagnostics;

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

        if (gameContainersNotFull.Count >= 10000)
        {
            return gameContainersNotFull[0];
        }

        GameContainer newGameContainer = new();
        StartGameContainer(newGameContainer.Id);
        _gameContainerRepository.Create(newGameContainer);

        return newGameContainer;
    }

    public GameContainer? ContainerAccessToJoinRoom(string roomCode, string userId)
    {
        CheckValid.JoinRoom(_roomPreviewRepository, _userRepository, userId);
        RoomPreview? roomPreview = _roomPreviewRepository.FindByCode(roomCode);
        if (roomPreview == null)
        {
            return null;
        }
        GameContainer gameContainer = _gameContainerRepository.FindById(roomPreview.ContainerId);
        return gameContainer;
    }

    public void StartGameContainer(string containerId)
    {
        //string strCmdText = "-c \"sshpass -p 'Iamroot!01' ssh localadm@adm.canardecarlate.fr -o StrictHostKeyChecking=no ./opt/Projet/ansible/run_container.sh " + containerId + "\"";
        string strCmdText = "-c \"sshpass -p 'Iamroot!01' ssh localadm@adm.canardecarlate.fr -o StrictHostKeyChecking=no cd /opt/Projet/ansible/ ls\"";

        ProcessStartInfo procStartInfo = new("/bin/bash", strCmdText)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process proc = new();
        proc.StartInfo = procStartInfo;
        proc.Start();

        string result = proc.StandardOutput.ReadToEnd();
        Console.WriteLine("coucou" + result);
    }
}