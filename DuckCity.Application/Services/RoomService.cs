using System.Security.Authentication;
using DuckCity.Application.Validations;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.Repositories;

namespace DuckCity.Application.Services;

public class RoomService
{
    private readonly UserRepository _userRepository;
    private readonly RoomRepository _roomRepository;

    public RoomService(UserRepository userRepository, RoomRepository roomRepository)
    {
        _userRepository = userRepository;
        _roomRepository = roomRepository;
    }

    public IEnumerable<Room> FindAllRooms() => _roomRepository.FindAllRooms();

    public Room FindRoom(string roomId) => _roomRepository.FindById(roomId);

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

    public Room JoinRooms(string roomId, string userId, string userName)
    {
        CheckValid.JoinRoom(_roomRepository, _userRepository, userId, roomId);
            
        Room room = _roomRepository.FindById(roomId);
        PlayerInRoom playerInRoom = new() {Id = userId, Name = userName};
        room.Players?.Add(playerInRoom);
        _roomRepository.Replace(room);
        return room;
    }

    public IEnumerable<PlayerInRoom> UpdatePlayerReadyInRoom(string userId, string roomId)
    {
        Room room = FindRoom(roomId);
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
}