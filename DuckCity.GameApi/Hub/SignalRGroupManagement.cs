using System.Collections.Immutable;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub
{
    public static class SignalRGroupManagement
    {
        // SignalRGroups = { string roomId, HashSet<string> connectionsIds }
        public static readonly Dictionary<string, HashSet<SignalRUser>?> SignalRGroups = new();

        public static async Task AddUser(HubCallerContext context, IGroupManager groups, string userId, string roomId)
        {
            SignalRUser signalRUser = new() {ContextId = context.ConnectionId, UserId = userId};

            // First, add in our group (created if first player in this group)
            if (SignalRGroups.TryGetValue(roomId, out HashSet<SignalRUser>? users))
            {
                users?.Add(signalRUser);
            }
            else
            {
                SignalRGroups.Add(roomId, new HashSet<SignalRUser> {signalRUser});
            }

            // Then add in the real group (not necessary to create when first player) 
            await groups.AddToGroupAsync(context.ConnectionId, roomId);
        }

        public static async Task RemoveUser(HubCallerContext context, IGroupManager groups, string roomId)
        {
            // Remove from our group first
            if (SignalRGroups.TryGetValue(roomId, out HashSet<SignalRUser>? value) && value != null)
            {
                value.RemoveWhere(c => c.ContextId == context.ConnectionId);
                if (!value.Any())
                {
                    SignalRGroups.Remove(roomId);
                }
            }

            // Remove from real group then
            await groups.RemoveFromGroupAsync(context.ConnectionId, roomId);
        }

        public static string FindUserRoom(HubCallerContext context, string userId)
        {
            SignalRUser signalRUser = new() {ContextId = context.ConnectionId, UserId = userId};
            string roomId = "";
            foreach ((string? key, HashSet<SignalRUser>? _) in SignalRGroups.Where(group =>
                         group.Value != null && group.Value.Contains(signalRUser)))
            {
                roomId = key;
            }

            return roomId;
        }
        
        public static IEnumerable<PlayerInRoom> UpdateConnectedPlayers(Room room)
        {
            if (SignalRGroups.TryGetValue(room.Id!, out HashSet<SignalRUser>? users))
            {
                foreach (PlayerInRoom player in room.Players!)
                {
                    player.Connected = users?.Where(u => u.UserId == player.Id) != null;
                }
            }

            return room.Players!;
        }

        public static IEnumerable<SignalRUser> ConnectedPlayers(string roomId)
        {
            return SignalRGroups.TryGetValue(roomId, out HashSet<SignalRUser>? users) ? users! : ImmutableHashSet<SignalRUser>.Empty;
        }
    }
}