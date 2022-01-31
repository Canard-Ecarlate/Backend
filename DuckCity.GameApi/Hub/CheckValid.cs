using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub
{
    public static class CheckValid
    {
        public static Room JoinSignalRGroup(IRoomService roomService, string roomId)
        {
            Room? room = roomService.FindRoom(roomId);
            if (room == null)
            {
                throw new RoomNotFoundException();
            }

            if (room.Players == null)
            {
                throw new PlayerNotFoundException();
            }

            return room;
        }

        public static Room? LeaveSignalRGroup(IRoomService roomService, string roomId)
        {
            return roomService.FindRoom(roomId);
        }

        public static void PlayerReady(HubCallerContext context, string userId, string roomId)
        {
            string roomIdInDb = SignalRGroupManagement.FindUserRoom(context, userId);
            if (string.IsNullOrEmpty(roomIdInDb) || !roomIdInDb.Equals(roomId))
            {
                throw new RoomIdNoExistException();
            }
        }
    }
}