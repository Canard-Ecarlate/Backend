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
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Find user
        SignalRUser signalRUser = SignalRUsers.FindSignalRUser(Context.ConnectionId);
        
        // Remove user from signalR
        await SignalRUsers.RemoveUser(Groups, signalRUser);
        
        // push connected players to group
        await Clients.Group(signalRUser.RoomId).PushPlayersInSignalRGroup(SignalRUsers.ConnectedPlayers(signalRUser.RoomId));
        
        await base.OnDisconnectedAsync(exception);
    }
    
    [HubMethodName("JoinSignalRGroup")]
    public async Task JoinSignalRGroupAsync(string roomId, string userId)
    {
        // Find room
        Room room = _roomService.FindRoom(roomId);
        
        // Add user in signalR
        await SignalRUsers.AddUser(Groups, Context.ConnectionId, roomId, userId);

        // Push players to group
        await Clients.Group(roomId).PushPlayersInRoom(SignalRUsers.UpdateConnectedPlayers(room));
    }

    [HubMethodName("LeaveRoomAndSignalRGroup")]
    public async Task LeaveRoomAndSignalRGroupAsync()
    {
        // Find user
        SignalRUser signalRUser = SignalRUsers.FindSignalRUser(Context.ConnectionId);
        
        // Leave room
        Room? room = _roomService.LeaveRoom(signalRUser.RoomId, signalRUser.UserId);
        
        // Remove user from signalR
        await SignalRUsers.RemoveUser(Groups, signalRUser);
        
        // If room still exists, push players to group
        if (room != null)
        {
            await Clients.Group(signalRUser.RoomId).PushPlayersInRoom(SignalRUsers.UpdateConnectedPlayers(room));
        }
    }

    [HubMethodName("PlayerReady")] 
    public async Task PlayerReadyAsync()
    {
        // Find user
        SignalRUser signalRUser = SignalRUsers.FindSignalRUser(Context.ConnectionId);

        // Update Ready property
        Room room = _roomService.UpdatedRoomReady(signalRUser.UserId, signalRUser.RoomId);
        
        // Push updated info to group
        await Clients.Group(signalRUser.RoomId).PushPlayersInRoom(SignalRUsers.UpdateConnectedPlayers(room));
    }
}