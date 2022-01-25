using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub
{
    public static class HubMessageSender
    {
        private const string PushMessageHubAsync = "PushMessageHubAsync";
        private const string PlayersUpToDateHubAsync = "PlayersUpToDateHubAsync";

        public static async Task HelloWorldToAll(IHubCallerClients clients, string user)
        {
            await clients.All.SendAsync(PushMessageHubAsync, $"Good Morning in GameApi {user}");
        }

        public static async Task AlertGroupOfDisconnection(HubCallerContext context, IHubCallerClients clients,
            string roomId)
        {
            await clients.Group(roomId).SendAsync(PushMessageHubAsync, $"Good Bye {context.ConnectionId}");
        }

        public static async Task AlertGroupOfPlayersUpToDate(HubCallerContext context, IHubCallerClients clients,
            string roomId, IEnumerable<PlayerInRoom> playersUpToDate)
        {
            await clients.Group(roomId).SendAsync(PlayersUpToDateHubAsync, playersUpToDate);
        }

        public static async Task AlertGroupOfUserLeft(HubCallerContext context, IHubCallerClients clients,
            string roomId)
        {
            await clients.Group(roomId).SendAsync(PushMessageHubAsync, $"Good Bye {context.ConnectionId} in {roomId}");
        }
    }
}