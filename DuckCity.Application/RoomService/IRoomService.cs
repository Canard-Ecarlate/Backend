﻿using DuckCity.Domain.Rooms;

namespace DuckCity.Application.RoomService;

public interface IRoomService
{
    void CreateRoom(Room newRoom);
    
    Room JoinRoom(string connectionId, string userId, string userName,string roomCode);

    Room? LeaveRoom(string roomCode, string connectionId);

    Room? DisconnectFromRoom(string connectionId);

    Room SetPlayerReady(string roomCode, string connectionId);
}