using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Repositories;
using DuckCity.Infrastructure.Repositories.Room;

namespace DuckCity.Application.Services.Room;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public Domain.Rooms.Room JoinRoomAndConnect(string connectionId, string userId, string userName, string roomId)
    {
        Domain.Rooms.Room? room = _roomRepository.FindRoomByRoomId(roomId);
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

    private Domain.Rooms.Room CreateRoom(string connectionId, string userId, string userName, string roomId)
    {
        Domain.Rooms.Room newRoom = new(roomId, connectionId, userId, userName);
        _roomRepository.Add(newRoom);
        return newRoom;
    }
    
    public Domain.Rooms.Room? LeaveRoomAndDisconnect(string roomId, string connectionId)
    {
        Domain.Rooms.Room room = _roomRepository.FindRoomByRoomId(roomId)!;
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        room.Players.Remove(player);

        if (room.Players is not {Count: 0})
        {
            return room;
        }
        _roomRepository.Remove(room);
        return null;
    }

    public Domain.Rooms.Room? DisconnectToRoom(string connectionId)
    {
        return _roomRepository.DisconnectPlayerFromRoom(connectionId);
    }
        
    public Domain.Rooms.Room SetPlayerReady(string roomId, string connectionId)
    {
        return _roomRepository.SetPlayerReadyInRoom(roomId, connectionId);
    }
}