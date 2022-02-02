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

    public Room FindRoom(string roomId)
    {
        CheckValid.IsObjectId(roomId);
        return _roomRepository.FindById(roomId);
    } 

    public Room AddRooms(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers)
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
        PlayerInRoom playerInRoom = new() {Id = userId, Name = userName}; 
        room.Players.Add(playerInRoom);
        _roomRepository.Replace(room);
        return room;
    }

    public Room UpdatedRoomReady(string userId, string roomId)
    {
        Room room = _roomRepository.FindById(roomId);
        IEnumerable<PlayerInRoom> playerInRooms = room.Players;
        PlayerInRoom playerInRoom = playerInRooms.Single(player => player.Id != null && player.Id.Equals(userId));
        playerInRoom.Ready = !playerInRoom.Ready;
        _roomRepository.Replace(room);
        return room;
    }

    public Room? LeaveRoom(string roomId, string userId)
    {
        Room room = _roomRepository.FindById(roomId);
        CheckValid.LeaveRoom(_userRepository, userId, room);
        room.Players.RemoveWhere(p => p.Id == userId);
        if (room.Players is {Count: 0})
        {
            _roomRepository.Delete(room);
            return null;
        }
        _roomRepository.Replace(room);
        return room;
    }
}