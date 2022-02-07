using AutoMapper;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Games;
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
        Game? game = _roomService.DisConnectToRoom(Context.ConnectionId);
        if (game != null)
        {
            // Send
            await Clients.Group(game.RoomId).PushPlayers(_mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(game.Players));
        }
        await base.OnDisconnectedAsync(exception);
    }
    
    [HubMethodName("JoinGameAndConnect")]
    public async Task JoinGameAndConnectAsync(string userId, string userName, string roomId)
    {
        Game game = _roomService.JoinGameAndConnect(Context.ConnectionId, userId, userName, roomId);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        // Send
        await Clients.Group(roomId).PushPlayers(_mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(game.Players));
    }

    [HubMethodName("LeaveGameAndDisconnect")]
    public async Task LeaveGameAndDisconnectAsync(string roomId)
    {
        Game? game = _roomService.LeaveGameAndDisconnect(roomId, Context.ConnectionId);
        if (game != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            // Send
            await Clients.Group(roomId).PushPlayers(_mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(game.Players));
        }
    }

    [HubMethodName("PlayerReady")]
    public async Task PlayerReadyAsync(string roomId)
    {
        Game game = _roomService.PlayerReady(roomId, Context.ConnectionId);

        // Send
        await Clients.Group(roomId).PushPlayers(_mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(game.Players));
    }
}