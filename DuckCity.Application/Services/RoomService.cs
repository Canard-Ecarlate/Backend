using DuckCity.Application.Services.Interfaces;
using DuckCity.Application.Validations;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Cache;
using DuckCity.Infrastructure.Repositories.Interfaces;

namespace DuckCity.Application.Services;

public class RoomService : IRoomService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerRepository _playerRepository;

    public RoomService(IUserRepository userRepository, IRoomRepository roomRepository, IPlayerRepository playerRepository)
    {
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _playerRepository = playerRepository;
    }

    public IEnumerable<Room> FindAllRooms() => _roomRepository.FindAllRooms();

    public Room FindRoom(string roomId)
    {
        CheckValid.IsObjectId(roomId);
        return _roomRepository.FindById(roomId);
    } 

    public Room CreateRoom(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers)
    {
        CheckValid.CreateRoom(_roomRepository, _userRepository, roomName, hostId);
        
        Room room = new(roomName, hostId, hostName, isPrivate, nbPlayers);
        _roomRepository.Create(room);
        
        return room;
    }

    public Room JoinRoom(string roomId, string userId, string userName)
    {
        CheckValid.JoinRoom(_roomRepository, _userRepository, userId);
        Room room = _roomRepository.FindById(roomId);
        
        // Add player in database
        room.PlayersId.Add(userId);

        _roomRepository.Replace(room);
        return room;
    }
    
    public string? LeaveAndDisconnectRoom(string connectionId)
    {
        Player player = _playerRepository.FindPlayerByConnectionId(connectionId);
        
        Room room = _roomRepository.FindById(player.RoomId);
        CheckValid.LeaveRoom(_userRepository, player.Id!, room);
        
        // Remove player in cache
        _playerRepository.RemovePlayer(player);
        
        // Remove player in database
        room.PlayersId.Remove(player.Id!);
        if (room.PlayersId is {Count: 0})
        {
            _roomRepository.Delete(room);
            return null;
        }
        _roomRepository.Replace(room);
        return room.Id;
    }

    public IEnumerable<Player> FindPlayersInRoom(string roomId)
    {
        return _playerRepository.FindPlayersInRoom(roomId);
    }

    public string SetReadyToPlayer(string connectionId)
    {
        return _playerRepository.ReadyToPlay(connectionId);
    }

    public void ConnectToRoom(string connectionId, string userId, string userName,string roomId)
    {
        // Add player in cache
        _playerRepository.AddOrReconnectPlayer(connectionId, userId, userName, roomId);
    }

    public string? DisConnectToRoom(string connectionId)
    {
        return _playerRepository.DisconnectToRoom(connectionId);
    }
}