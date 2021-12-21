using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hubs
{
    public static class MessageSender
    {
        private const string PushMessageAsync = "PushMessageAsync";

        public static async Task HelloWorldToAll(IHubCallerClients clients, string user)
        {
            await clients.All.SendAsync (PushMessageAsync, $"Good Morning in GameApi {user}");
        }
        
        public static async Task AlertGroupOfDisconnection(HubCallerContext context, IHubCallerClients clients,
            string roomName)
        {
            await clients.Group(roomName).SendAsync(PushMessageAsync, $"Good Bye {context.ConnectionId}");
        }

        public static async Task AlertGroupOfNewUser(HubCallerContext context, IHubCallerClients clients,
            string roomName)
        {
            await clients.Group(roomName).SendAsync (PushMessageAsync, $"Good Morning {context.ConnectionId} in {roomName}");
        }
        
        public static async Task AlertGroupOfUserLeft(HubCallerContext context, IHubCallerClients clients,
            string roomName)
        {
            await clients.Group(roomName).SendAsync (PushMessageAsync, $"Good Bye {context.ConnectionId} in {roomName}");
        }
    }
}