using AutoMapper;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Users;
using DuckCity.GameApi.Dto;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub;

public class DuckCityHub : Hub<IDuckCityClient>
{
    private readonly IRoomService _roomService;
    private readonly IMapper _mapper;

    // Constructor
    public DuckCityHub(IRoomService roomService, IMapper mapper)
    {
        _roomService = roomService;
        _mapper = mapper;
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
            IEnumerable<Player> players = _roomService.FindPlayersInRoom(roomId);
            IEnumerable<PlayerInWaitingRoomDto> playersInRoom = _mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(players);
            await Clients.Group(roomId).PushPlayers(playersInRoom);
        }
        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName("ConnectOrReconnectPlayer")]
    public async Task ConnectOrReconnectPlayerAsync(string userId, string userName, string roomId)
    {
        _roomService.ConnectOrReconnectPlayer(Context.ConnectionId, userId, userName, roomId);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        // Send
        IEnumerable<Player> players = _roomService.FindPlayersInRoom(roomId);
        IEnumerable<PlayerInWaitingRoomDto> playersInRoom = _mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(players);
        await Clients.Group(roomId).PushPlayers(playersInRoom);
    }

    [HubMethodName("DisconnectPlayerAndLeaveRoom")]
    public async Task DisconnectPlayerAndLeaveRoomAsync()
    {
        string? roomId = _roomService.DisconnectPlayerAndLeaveRoom(Context.ConnectionId);
        if (roomId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            // Send
            IEnumerable<Player> players = _roomService.FindPlayersInRoom(roomId);
            IEnumerable<PlayerInWaitingRoomDto> playersInRoom = _mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(players);
            await Clients.Group(roomId).PushPlayers(playersInRoom);
        }
    }

    [HubMethodName("PlayerReady")]
    public async Task PlayerReadyAsync()
    {
        string roomId = _roomService.PlayerReady(Context.ConnectionId);

        // Send
        IEnumerable<Player> players = _roomService.FindPlayersInRoom(roomId);
        IEnumerable<PlayerInWaitingRoomDto> playersInRoom = _mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(players);
        await Clients.Group(roomId).PushPlayers(playersInRoom);
    }
}