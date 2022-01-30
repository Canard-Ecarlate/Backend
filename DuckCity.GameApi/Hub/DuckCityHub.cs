using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.GameApi.DTO;

namespace DuckCity.GameApi.Hub;

public class DuckCityHub : Microsoft.AspNetCore.SignalR.Hub
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
        string roomName = HubGroupManagement.FindUserRoom(Context);
        await HubGroupManagement.RemoveUser(Context, Groups, roomName);
        await HubMessageSender.AlertGroupOfDisconnection(Context, Clients, roomName);
        await base.OnDisconnectedAsync(exception);
    }

    /**
     * Methods
     */
    public async Task SendMessageHubAsync(string user)
    {
        await HubMessageSender.HelloWorldToAll(Clients, user);
    }

    public async Task JoinSignalRGroupHubAsync(string roomId)
    {
        await HubGroupManagement.AddUser(Context, Groups, roomId);
        Room? room = _roomService.FindRoom(roomId);
        if (room != null)
        {
            if (room.Players == null)
            {
                throw new PlayerNotFoundException();
            }
            await HubMessageSender.AlertGroupOfPlayersUpToDate(Context, Clients, roomId, room.Players);
        }
    }

    public async Task LeaveSignalRGroupHubAsync(string roomId)
    {
        await HubGroupManagement.RemoveUser(Context, Groups, roomId);
        Room? room = _roomService.FindRoom(roomId);
        if (room != null)
        {
            if (room.Players == null)
            {
                throw new PlayerNotFoundException();
            }
            await HubMessageSender.AlertGroupOfPlayersUpToDate(Context, Clients, roomId, room.Players);
        }
    }

    public async Task PlayerReadyHubAsync(UserIdAndRoomIdDto userIdAndRoomIdDto)
    {
        // Validations roomId is a signalR group and User is in
        string roomId = HubGroupManagement.FindUserRoom(Context);
        if (string.IsNullOrEmpty(roomId) || !roomId.Equals(userIdAndRoomIdDto.RoomId))
        {
            throw new RoomIdNoExistException();
        }

        // update list of players and send it
        IEnumerable<PlayerInRoom> playersUpToDate =
            _roomService.UpdatePlayerReadyInRoom(userIdAndRoomIdDto.UserId, userIdAndRoomIdDto.RoomId);
        await HubMessageSender.AlertGroupOfPlayersUpToDate(Context, Clients, userIdAndRoomIdDto.RoomId, playersUpToDate);
    }
}