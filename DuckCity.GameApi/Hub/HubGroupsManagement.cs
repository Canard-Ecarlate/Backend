using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub
{
    public static class HubGroupManagement
    {
        // SignalRGroups =
        // {
        //    string roomId,
        //    HashSet<string> connectionsIds
        // }
        private static readonly Dictionary<string, HashSet<string>?> SignalRGroups = new();

        public static async Task AddUser(HubCallerContext context, IGroupManager groups, string roomId)
        {
            // First, add in our SignalRGroups (group created if first player in this group)
            bool hasValue = SignalRGroups.TryGetValue(roomId, out HashSet<string>? connectionIds);
            if (hasValue)
            {
                connectionIds?.Add(context.ConnectionId);
            }
            else
            {
                HashSet<string> newConnectionIds = new() {context.ConnectionId};
                SignalRGroups.Add(roomId, newConnectionIds);
            }

            // Then add in the real group (not necessary to create when first player) 
            await groups.AddToGroupAsync(context.ConnectionId, roomId);
        }

        public static async Task RemoveUser(HubCallerContext context, IGroupManager groups, string roomId)
        {
            // Remove from signalR group first
            if (SignalRGroups.TryGetValue(roomId, out HashSet<string>? value) && value != null)
            {
                value.Remove(context.ConnectionId);
                if (!value.Any()) // If empty, remove this signalr group
                {
                    SignalRGroups.Remove(roomId);
                }
            }

            // Remove from real group then
            await groups.RemoveFromGroupAsync(context.ConnectionId, roomId);
        }

        public static string FindUserRoom(HubCallerContext context)
        {
            string roomId = "";
            foreach ((string? key, HashSet<string>? _) in SignalRGroups.Where(group =>
                group.Value != null && group.Value.Contains(context.ConnectionId)))
            {
                roomId = key;
            }
            return roomId;
        }
    }
}