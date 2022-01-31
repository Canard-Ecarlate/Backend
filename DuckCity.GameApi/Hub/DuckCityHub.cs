using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub;

public class DuckCityHub : Hub<IDuckCityClient>
{
    private readonly IRoomService _roomService;

    public DuckCityHub(IRoomService roomService)
    {
        _roomService = roomService;
    }

    /**
     * Life cycle of signalR's users
     */
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string roomId = SignalRGroupManagement.FindUserRoom(Context, string.Empty);
        await SignalRGroupManagement.RemoveUser(Context, Groups, roomId);
        await Clients.Group(roomId).PushPlayersInSignalRGroup(SignalRGroupManagement.ConnectedPlayers(roomId));
        await base.OnDisconnectedAsync(exception);
    }

    /**
     * Methods
     */
    public async Task SendMessageHubAsync(string user)
    {
        await Clients.All.PushMessage($"Good Morning in GameApi {user}");
    }

    [HubMethodName("JoinSignalRGroup")] 
    public async Task JoinSignalRGroupAsync(string userId, string roomId)
    {
        await SignalRGroupManagement.AddUser(Context, Groups, userId, roomId);
        Room room = CheckValid.JoinSignalRGroup(_roomService, roomId);
        await Clients.Group(roomId).PushPlayersInRoom(SignalRGroupManagement.UpdateConnectedPlayers(room));
    }

    [HubMethodName("LeaveSignalRGroup")]
    public async Task LeaveSignalRGroupAsync(string roomId)
    {
        await SignalRGroupManagement.RemoveUser(Context, Groups, roomId);
        Room? room = CheckValid.LeaveSignalRGroup(_roomService, roomId);
        if (room != null)
        {
            await Clients.Group(roomId).PushPlayersInRoom(SignalRGroupManagement.UpdateConnectedPlayers(room));
        }
    }

    [HubMethodName("PlayerReady")] 
    public async Task PlayerReadyAsync(string userId, string roomId)
    {
        CheckValid.PlayerReady(Context, userId, roomId);
        Room room = _roomService.UpdatedRoomReady(userId, roomId);
        await Clients.Group(roomId).PushPlayersInRoom(SignalRGroupManagement.UpdateConnectedPlayers(room));
    }
}