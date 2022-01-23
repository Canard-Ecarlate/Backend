using DuckCity.GameApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub
{
    public static class HubMessageSender
    {
        private const string PushMessageHubAsync = "PushMessageHubAsync";

        public static async Task HelloWorldToAll(IHubCallerClients clients, string user)
        {
            await clients.All.SendAsync(PushMessageHubAsync, $"Good Morning in GameApi {user}");
        }

        public static async Task AlertGroupOfDisconnection(HubCallerContext context, IHubCallerClients clients,
            string roomId)
        {
            await clients.Group(roomId).SendAsync(PushMessageHubAsync, $"Good Bye {context.ConnectionId}");
        }

        public static async Task AlertGroupOfNewUser(HubCallerContext context, IHubCallerClients clients,
            string roomId)
        {
            await clients.Group(roomId)
                .SendAsync(PushMessageHubAsync, $"Good Morning {context.ConnectionId} in {roomId}");
        }

        public static async Task AlertGroupOfPlayerReady(HubCallerContext context, IHubCallerClients clients,
            UserAndRoom userAndRoom)
        {
            // A terme, doit envoyer la liste des joueurs prets 
            await clients.Group(userAndRoom.RoomId).SendAsync(PushMessageHubAsync,
                $"Ready to play of {userAndRoom.UserId} in {userAndRoom.RoomId}");
        }

        public static async Task AlertGroupOfUserLeft(HubCallerContext context, IHubCallerClients clients,
            string roomId)
        {
            await clients.Group(roomId).SendAsync(PushMessageHubAsync, $"Good Bye {context.ConnectionId} in {roomId}");
        }
    }
}