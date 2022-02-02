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
            HashSet<SignalRUser> users = ConnectedPlayers(room.Id);
            foreach (PlayerInRoom player in room.Players)
            {
                SignalRUser? signalRUser = users.SingleOrDefault(u => u.UserId == player.Id);
                if (signalRUser != null)
                {
                    player.Connected = true;
                }
            }

            return room.Players;
        }

        public static HashSet<SignalRUser> ConnectedPlayers(string roomId)
        {
            return List.Where(u => u.RoomId == roomId).ToHashSet();
        }
    }
}