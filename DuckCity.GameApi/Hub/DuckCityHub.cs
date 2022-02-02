using DuckCity.Application.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub;

public class DuckCityHub : Hub<IDuckCityClient>
{
    private readonly IRoomService _roomService;

    // Constructor
    public DuckCityHub(IRoomService roomService)
    {
        _roomService = roomService;
    }

    /**
     * Methods
     */
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string? roomId = _roomService.DisConnectToRoom(Context.ConnectionId);
        if (roomId != null)
        {
            // Send
            await Clients.Group(roomId).PushPlayers(_roomService.FindPlayersInRoom(roomId));
        }
        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName("ConnectToRoom")]
    public async Task ConnectToRoomAsync(string userId, string userName, string roomId)
    {
        _roomService.ConnectToRoom(Context.ConnectionId, userId, userName, roomId);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        // Send
        await Clients.Group(roomId).PushPlayers(_roomService.FindPlayersInRoom(roomId));
    }

    [HubMethodName("LeaveRoomAndDisconnect")]
    public async Task LeaveRoomAndDisconnectAsync()
    {
        string? roomId = _roomService.LeaveAndDisconnectRoom(Context.ConnectionId);
        if (roomId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
                   
            // Send
            await Clients.Group(roomId).PushPlayers(_roomService.FindPlayersInRoom(roomId));
        }
    }

    [HubMethodName("PlayerReady")]
    public async Task PlayerReadyAsync()
    {
        string roomId = _roomService.SetReadyToPlayer(Context.ConnectionId);

        // Send
        await Clients.Group(roomId).PushPlayers(_roomService.FindPlayersInRoom(roomId));
    }
}