using AutoMapper;
using DuckCity.Application.Services.Room;
using DuckCity.Application.Services.RoomPreview;
using DuckCity.Domain.Rooms;
using DuckCity.GameApi.Dto;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub;

public class DuckCityHub : Hub<IDuckCityClient>
{
    private readonly IRoomService _roomService;
    private readonly IRoomPreviewService _roomPreviewService;
    private readonly IMapper _mapper;

    // Constructor
    public DuckCityHub(IRoomService roomService, IMapper mapper, IRoomPreviewService roomPreviewService)
    {
        _roomService = roomService;
        _mapper = mapper;
        _roomPreviewService = roomPreviewService;
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
        Room? room = _roomService.DisconnectToRoom(Context.ConnectionId);
        if (room != null)
        {
            // Send
            await Clients.Group(room.RoomId).PushPlayers(_mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
        }
        await base.OnDisconnectedAsync(exception);
    }
    
    [HubMethodName("JoinRoomAndConnect")]
    public async Task JoinRoomAndConnectAsync(string roomId, string userId, string userName)
    {
        Room room = _roomService.JoinRoomAndConnect(Context.ConnectionId, userId, userName, roomId);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        // Send
        await Clients.Group(roomId).PushPlayers(_mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
    }

    [HubMethodName("LeaveRoomAndDisconnect")]
    public async Task LeaveRoomAndDisconnectAsync(string roomId, string userId)
    {
        RoomPreview? roomPreview = _roomPreviewService.LeaveRoomPreview(roomId, userId);
        Room? room = _roomService.LeaveRoomAndDisconnect(roomId, Context.ConnectionId);
        if (room != null && roomPreview != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            // Send
            await Clients.Group(roomId).PushPlayers(_mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
        }
    }

    [HubMethodName("PlayerReady")]
    public async Task PlayerReadyAsync(string roomId)
    {
        Room room = _roomService.SetPlayerReady(roomId, Context.ConnectionId);

        // Send
        await Clients.Group(roomId).PushPlayers(_mapper.Map <IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
    }
}