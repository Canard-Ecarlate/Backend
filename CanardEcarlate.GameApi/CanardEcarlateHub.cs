using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CanardEcarlate.GameApi
{
    public class CanardEcarlateHub : Hub
    {
        private static readonly Dictionary<string, HashSet<string>> MyGroups = new();
        private const string AFTER_SEND_MESSAGE_ASYNC = "AfterSendMessageAsync";

        public async Task SendMessageAsync (string user) {
            await Clients.All.SendAsync (AFTER_SEND_MESSAGE_ASYNC, $"Good Morning in GameApi {user}");
        }
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string roomName = "";
            foreach (KeyValuePair<string, HashSet<string>> group in MyGroups.Where(group => group.Value.Contains(Context.ConnectionId)))
            {
                roomName = group.Key;
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync (AFTER_SEND_MESSAGE_ASYNC, $"Good Bye {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoomAsync(string roomName)
        {
            // Group management for alerting players if a disconnection appears : if group exists, add connectionId, if not create group and add connectionId
            bool hasValue = MyGroups.TryGetValue(roomName, out HashSet<string> value);
            if (hasValue)
            {
                value.Add(Context.ConnectionId);
            }
            else
            {
                HashSet<string> connectionIds = new HashSet<string> {Context.ConnectionId};
                MyGroups.Add(roomName, connectionIds);
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync (AFTER_SEND_MESSAGE_ASYNC, $"Good Morning {Context.ConnectionId} in {roomName}");
        }

        public async Task LeaveRoomAsync(string roomName)
        {
            // Group management for alerting players if a disconnection appears : if group exists, add connectionId, if not create group and add connectionId
            if (MyGroups.TryGetValue(roomName, out HashSet<string> value))
            {
                value.Remove(Context.ConnectionId);
            }

            await Clients.Group(roomName).SendAsync (AFTER_SEND_MESSAGE_ASYNC, $"Good Bye {Context.ConnectionId} in {roomName}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}