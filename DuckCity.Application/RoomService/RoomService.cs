using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.RoomRepository;

namespace DuckCity.Application.RoomService;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public Room JoinRoomAndConnect(string connectionId, string userId, string userName, string roomId)
    {
        Room? room = _roomRepository.FindById(roomId);
        if (room == null)
        {
            return CreateRoom(connectionId, userId, userName, roomId);
        }
        Player? player = room.Players.SingleOrDefault(p => p.Id == userId);
        if (player == null)
        {
            room.Players.Add(new Player(connectionId, userId, userName));
        }
        else
        {
            player.ConnectionId = connectionId;
        }
        _roomRepository.Update(room);
        return room;
    }

    private Room CreateRoom(string connectionId, string userId, string userName, string roomId)
    {
        Room newRoom = new(roomId, connectionId, userId, userName);
        _roomRepository.Create(newRoom);
        return newRoom;
    }
    
    public Room? LeaveRoomAndDisconnect(string roomId, string connectionId)
    {
        Room room = _roomRepository.FindById(roomId)!;
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        room.Players.Remove(player);
        if (room.Players is not {Count: 0})
        {
            _roomRepository.Update(room);
            return room;
        }
        _roomRepository.Delete(room);
        return null;
    }

    public Room? DisconnectFromRoom(string connectionId)
    {
        Room? room = _roomRepository.FindByConnectionId(connectionId);
        if (room == null)
        {
            return room;
        }
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        player.ConnectionId = null;
        _roomRepository.Update(room);
        return room;
    }
        
    public Room SetPlayerReady(string roomId, string connectionId)
    {
        Room room = _roomRepository.FindById(roomId)!;
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        player.Ready = !player.Ready;
        _roomRepository.Update(room);
        return room;
    }
}