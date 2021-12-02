using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CanardEcarlate.Api.Models
{
    public class CanardEcarlateHub : Hub
    {
        public async Task SendMessageAsync (string user) {
            await Clients.All.SendAsync ("AfterSendMessageAsync", $"Good Morning {user}");
        }
        
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
        
        public async Task JoinRoomAsync(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync ("AfterSendMessageAsync", $"Good Morning {Context.ConnectionId} in {roomName}");
        }

        public async Task LeaveRoomAsync(string roomName)
        {
            await Clients.Group(roomName).SendAsync ("AfterSendMessageAsync", $"Good Bye {Context.ConnectionId} in {roomName}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}