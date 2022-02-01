using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub
{
    public static class SignalRUsers
    {
        public static HashSet<SignalRUser> List { get; } = new();

        public static async Task AddUser(IGroupManager groups, string connectionId, string roomId, string userId)
        {
            List.Add(new SignalRUser {ConnectionId = connectionId, RoomId = roomId, UserId = userId});
            await groups.AddToGroupAsync(connectionId, roomId);
        }
        
        public static async Task RemoveUser(IGroupManager groups, SignalRUser signalRUser)
        {
            List.Remove(signalRUser);
            await groups.RemoveFromGroupAsync(signalRUser.ConnectionId, signalRUser.RoomId);
        }
        
        public static SignalRUser FindSignalRUser(string connectionId)
        {
            return List.First(u => u.ConnectionId == connectionId);
        }
        
        public static IEnumerable<PlayerInRoom> UpdateConnectedPlayers(Room room)
        {
            IEnumerable<SignalRUser> users = List.Where(u => u.RoomId == room.Id);
            foreach (PlayerInRoom player in room.Players)
            {
                player.Connected = users.Count(u => u.UserId == player.Id) == 1;
            }

            return room.Players!;
        }

        public static IEnumerable<SignalRUser> ConnectedPlayers(string roomId)
        {
            return List.Where(u => u.RoomId == roomId);
        }
    }
}