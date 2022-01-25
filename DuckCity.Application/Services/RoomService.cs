using DuckCity.Application.Services.Interfaces;
using DuckCity.Application.Validations;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.Repositories.Interfaces;

namespace DuckCity.Application.Services;

public class RoomService : IRoomService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;

    public RoomService(IUserRepository userRepository, IRoomRepository roomRepository)
    {
        _userRepository = userRepository;
        _roomRepository = roomRepository;
    }

    public IEnumerable<Room> FindAllRooms() => _roomRepository.FindAllRooms();

    public Room? FindRoom(string roomId) => _roomRepository.FindById(roomId);

    public Room AddRooms(string? roomName, string? hostId, string? hostName, bool isPrivate, int nbPlayers)
    {
        CheckValid.CreateRoom(_userRepository, roomName, hostId);
        Room room = new()
        {
            Name = roomName,
            Code = "to do : random code",
            HostId = hostId,
            HostName = hostName,
            ContainerId = "to do : a container id",
            RoomConfiguration = new RoomConfiguration(isPrivate, nbPlayers),
            Players = new HashSet<PlayerInRoom>()
        };
        _roomRepository.Create(room);
        return room;
    }

    public Room JoinRoom(string roomId, string userId, string userName)
    {
        CheckValid.JoinRoom(_roomRepository, _userRepository, userId, roomId);
            
        Room? room = _roomRepository.FindById(roomId);
        if (room == null)
        {
            throw new RoomNotFoundException();
        }
        if (room.Players == null)
        {
            throw new PlayersNotFoundException();
        }
        PlayerInRoom playerInRoom = new() {Id = userId, Name = userName}; 
        room.Players.Add(playerInRoom);
        _roomRepository.Replace(room);
        return room;
    }

    public IEnumerable<PlayerInRoom> UpdatePlayerReadyInRoom(string userId, string roomId)
    {
        Room? room = _roomRepository.FindById(roomId);
        if (room == null)
        {
            throw new RoomNotFoundException();
        }
        if (room.Players == null)
        {
            throw new PlayersNotFoundException();
        }
        IEnumerable<PlayerInRoom> playerInRooms = room.Players;
        PlayerInRoom? playerInRoom = playerInRooms.SingleOrDefault(player => player.Id != null && player.Id.Equals(userId));
        if (playerInRoom == null)
        {
            throw new PlayerNotFoundException();
        }
        playerInRoom.Ready = !playerInRoom.Ready;
        _roomRepository.Replace(room);
        return playerInRooms;
    }

    public bool LeaveRoom(string roomId, string userId)
    {
        Room room = CheckValid.LeaveRoom(_roomRepository, _userRepository, userId, roomId);
        room.Players?.RemoveWhere(p => p.Id == userId);
        if (room.Players is {Count: 0})
        {
            _roomRepository.Delete(room);
        }
        else
        {
            _roomRepository.Replace(room);
        }
        return true;
    }
}