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
        Room? room = _roomRepository.FindRoomByRoomId(roomId);
        if (room == null)
        {
            return CreateRoom(connectionId, userId, userName, roomId);
        }
        Player? player = room.Players.SingleOrDefault(p => p.Id == userId);
        if (player == null)
        {
            Player newPlayer = new(connectionId, userId, userName);
            room.Players.Add(newPlayer);
        }
        else
        {
            player.ConnectionId = connectionId;
        }

        return room;
    }

    private Room CreateRoom(string connectionId, string userId, string userName, string roomId)
    {
        Room newRoom = new(roomId, connectionId, userId, userName);
        _roomRepository.Add(newRoom);
        return newRoom;
    }
    
    public Room? LeaveRoomAndDisconnect(string roomId, string connectionId)
    {
        Room room = _roomRepository.FindRoomByRoomId(roomId)!;
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        room.Players.Remove(player);

        if (room.Players is not {Count: 0})
        {
            return room;
        }
        _roomRepository.Remove(room);
        return null;
    }

    public Room? DisconnectFromRoom(string connectionId)
    {
        return _roomRepository.DisconnectPlayerFromRoom(connectionId);
    }
        
    public Room SetPlayerReady(string roomId, string connectionId)
    {
        return _roomRepository.SetPlayerReadyInRoom(roomId, connectionId);
    }
}