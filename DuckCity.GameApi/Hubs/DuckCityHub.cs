namespace DuckCity.GameApi.Hubs
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
            string roomName = GroupManagement.FindUserRoom(Context);
            await GroupManagement.RemoveUser(Context, Groups, roomName);
            await MessageSender.AlertGroupOfDisconnection(Context, Clients, roomName);
            await base.OnDisconnectedAsync(exception);
        }

        // Methods
        public async Task SendMessageAsync (string user)
        {
            await MessageSender.HelloWorldToAll(Clients, user);
        }

        public async Task JoinRoomAsync(string roomName)
        {
            await GroupManagement.AddUser(Context, Groups, roomName);
            await MessageSender.AlertGroupOfNewUser(Context, Clients, roomName);
        }

        public async Task LeaveRoomAsync(string roomName)
        {
            await GroupManagement.RemoveUser(Context, Groups, roomName);
            await MessageSender.AlertGroupOfUserLeft(Context, Clients, roomName);
        }
    }
}