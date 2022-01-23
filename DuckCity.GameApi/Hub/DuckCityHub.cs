using DuckCity.Application.Services;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.GameApi.Models;

namespace DuckCity.GameApi.Hub;

public class DuckCityHub : Microsoft.AspNetCore.SignalR.Hub
{
   
    private readonly RoomService _roomService;

    public DuckCityHub(RoomService roomService)
    {
        _roomService = roomService;
    }

     
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
        Room room = _roomService.GetRoom(roomId);
        if (room.Players == null)
        {
            throw new PlayerNotFoundException();
        }
        await HubMessageSender.AlertGroupOfPlayersUpToDate(Context, Clients, roomId, room.Players);
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
        if (string.IsNullOrEmpty(roomId) || !roomId.Equals(userAndRoom.RoomId))
        {
            throw new RoomIdNoExistException();
        }
        // update list of players and send it
        IEnumerable<PlayerInRoom> playersUpToDate =
            _roomService.UpdatePlayerReadyInRoom(userAndRoom.UserId, userAndRoom.RoomId);
        await HubMessageSender.AlertGroupOfPlayersUpToDate(Context, Clients, userAndRoom.RoomId, playersUpToDate);
  }
}