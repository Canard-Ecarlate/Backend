using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hubs
{
    public static class GroupManagement
    {
        private static readonly Dictionary<string, HashSet<string>?> MyGroups = new();

        public static async Task AddUser(HubCallerContext context, IGroupManager groups, string roomName)
        {
            bool hasValue = MyGroups.TryGetValue(roomName, out HashSet<string>? value);
            if (hasValue)
            {
                value?.Add(context.ConnectionId);
            }
            else
            {
                HashSet<string> connectionIds = new() {context.ConnectionId};
                MyGroups.Add(roomName, connectionIds);
            }

            await groups.AddToGroupAsync(context.ConnectionId, roomName);
        }

        public static async Task RemoveUser(HubCallerContext context, IGroupManager groups, string roomName)
        {
            if (MyGroups.TryGetValue(roomName, out HashSet<string>? value))
            {
                value?.Remove(context.ConnectionId);
            }

            await groups.RemoveFromGroupAsync(context.ConnectionId, roomName);
        }

        public static string FindUserRoom(HubCallerContext context)
        {
            string roomName = "";
            foreach ((string? key, HashSet<string>? _) in MyGroups.Where(group =>
                group.Value != null && group.Value.Contains(context.ConnectionId)))
            {
                roomName = key;
            }
            return roomName;
        }
    }
}