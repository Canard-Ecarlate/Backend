using DuckCity.Application.Services.Interfaces;
using DuckCity.Application.Validations;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Repositories;

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

    public Room CreateAndJoinRoom(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers)
    {
        CheckValid.CreateRoom(_roomRepository, _userRepository, roomName, hostId);
        
        Room room = new(roomName, hostId, hostName, isPrivate, nbPlayers);
        _roomRepository.Create(room);
        
        return room;
    }

    public Room JoinRoom(string roomId, string userId, string userName)
    {
        CheckValid.JoinRoom(_roomRepository, _userRepository, userId);
        Room room = _roomRepository.JoinRoom(roomId, userId);
        return room;
    }
    
    public string? DisconnectPlayerAndLeaveRoom(string connectionId)
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

    public string PlayerReady(string connectionId)
    {
        return _playerRepository.PlayerReady(connectionId);
    }

    public void ConnectOrReconnectPlayer(string connectionId, string userId, string userName,string roomId)
    {
        Player? player = _playerRepository.FindPlayerByUserId(userId);
        if (player != null)
        {
            _playerRepository.ReconnectPlayer(player, connectionId);
        }
        else
        {
            _playerRepository.ConnectPlayer(connectionId, userId, userName, roomId);
        }
    }

    public string? DisConnectToRoom(string connectionId)
    {
        return _playerRepository.DisconnectPlayer(connectionId);
    }
}