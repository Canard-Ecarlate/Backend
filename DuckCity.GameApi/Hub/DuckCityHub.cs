using DuckCity.GameApi.Models;

namespace DuckCity.GameApi.Hub
{
    public class DuckCityHub : Microsoft.AspNetCore.SignalR.Hub
    {
        // Life cycle of signalR's users
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string roomName = HubGroupManagement.FindUserRoom(Context);
            await HubGroupManagement.RemoveUser(Context, Groups, roomName);
            await HubMessageSender.AlertGroupOfDisconnection(Context, Clients, roomName);
            await base.OnDisconnectedAsync(exception);
        }

        // Methods
        public async Task SendMessageHubAsync (string user)
        {
            await HubMessageSender.HelloWorldToAll(Clients, user);
        }

        public async Task JoinSignalRGroupHubAsync(string roomId)
        {
            await HubGroupManagement.AddUser(Context, Groups, roomId);
            await HubMessageSender.AlertGroupOfNewUser(Context, Clients, roomId);
            // update player list of <roomId>
        }

        public async Task LeaveSignalRGroupHubAsync(string roomId)
        {
            await HubGroupManagement.RemoveUser(Context, Groups, roomId);
            await HubMessageSender.AlertGroupOfUserLeft(Context, Clients, roomId);
        }

        public async Task PlayerReadyHubAsync(UserAndRoom userAndRoom)
        {
            // Validations roomId is a signalR group and User is in
            string roomId = HubGroupManagement.FindUserRoom(Context);
            if (!string.IsNullOrEmpty(roomId) && roomId.Equals(userAndRoom.RoomId))
            {
                // update list of players ready
                
                // send list updated to alert other members of group
                await HubMessageSender.AlertGroupOfPlayerReady(Context, Clients, userAndRoom);
            }
        }
    }
}