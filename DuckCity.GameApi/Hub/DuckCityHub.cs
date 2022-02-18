using AutoMapper;
using DuckCity.Application.GameService;
using DuckCity.Application.RoomPreviewService;
using DuckCity.Application.RoomService;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Games;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.GameApi.Dto;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub;

public class DuckCityHub : Hub<IDuckCityClient>
{
    // symbole à mettre à la fin des requête sur websocket king : 
    private readonly IRoomService _roomService;
    private readonly IGameService _gameService;
    private readonly IRoomPreviewService _roomPreviewService;
    private readonly IMapper _mapper;

    // Constructor
    public DuckCityHub(IRoomService roomService, IGameService gameService, IMapper mapper, IRoomPreviewService roomPreviewService)
    {
        _roomService = roomService;
        _gameService = gameService;
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
        Room? room = _roomService.DisconnectFromRoom(Context.ConnectionId);
        if (room != null)
        {
            // Send
            await Clients.Group(room.Code).PushPlayers(_mapper.Map<IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
        }

        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName("CreateRoom")]
    public async Task CreateRoomAsync(RoomCreationDto roomDto)
    {
        Room newRoom = new(roomDto.RoomName, roomDto.HostId, roomDto.HostName, roomDto.ContainerId, roomDto.IsPrivate,
            roomDto.NbPlayers, Context.ConnectionId, _roomPreviewService.GenerateCode());
        
        // Create Room
        _roomService.CreateRoom(newRoom);
        
        // Create RoomPreview from Room
        _roomPreviewService.CreateRoomPreview(new RoomPreview(newRoom));
        
        // Join SignalR
        await Groups.AddToGroupAsync(Context.ConnectionId, newRoom.Code);

        // Send
        await Clients.Group(newRoom.Code).PushPlayers(_mapper.Map<IEnumerable<PlayerInWaitingRoomDto>>(newRoom.Players));
    }


    [HubMethodName("JoinRoom")]
    public async Task JoinRoomAsync(string roomCode, string userId, string userName)
    {
        // Join Room
        Room room = _roomService.JoinRoom(Context.ConnectionId, userId, userName, roomCode);
       
        // Update RoomPreview from Room
        _roomPreviewService.UpdateRoomPreview(new RoomPreview(room));
        
        // Join SignalR
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

        // Send
        await Clients.Group(roomCode).PushPlayers(_mapper.Map<IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
    }

    [HubMethodName("LeaveRoom")]
    public async Task LeaveRoomAsync(string roomCode, string userId)
    {
        // Leave Room
        Room? room = _roomService.LeaveRoom(roomCode, Context.ConnectionId);

        // Leave SignalR
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);

        if (room != null)
        {
            // Update RoomPreview
            _roomPreviewService.UpdateRoomPreview(new RoomPreview(room));
            
            // Send
            await Clients.Group(roomCode).PushPlayers(_mapper.Map<IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
        }
        else
        {
            // Delete RoomPreview
            _roomPreviewService.DeleteRoomPreview(roomCode);
        }
    }

    [HubMethodName("PlayerReady")]
    public async Task PlayerReadyAsync(string roomCode)
    {
        Room room = _roomService.SetPlayerReady(roomCode, Context.ConnectionId);

        // Send
        await Clients.Group(roomCode).PushPlayers(_mapper.Map<IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
    }

    [HubMethodName("StartGame")]
    public async Task StartGameAsync(string roomId)
    {
        Room room = _gameService.StartGame(roomId);
        await SendGameInfo(room);
    }

    [HubMethodName("DrawCard")]
    public async Task DrawCardAsync(string roomId, string playerWhereCardIsDrawingId)
    {
        Room room = _gameService.DrawCard(Context.ConnectionId, playerWhereCardIsDrawingId, roomId);
        await SendGameInfo(room);
    }

    [HubMethodName("QuitMidGame")]
    public async Task QuitMidGameAsync(string roomId)
    {
        _gameService.QuitMidGame(roomId);

        await LeaveRoomAsync(roomId, Context.ConnectionId);
    }

    public async Task SendGameInfo(Room room)
    {
        if (room.Game == null)
        {
            throw new GameNotBeginException();
        }
        Game game = room.Game;
        HashSet<string> playersWithCardsDrawable = new(room.Players.Where(p => p.IsCardsDrawable).Select(p => p.Id));
        foreach (Player player in room.Players)
        {
            if (player.ConnectionId != null)
            {
                if (player.Role == null)
                {
                    throw new GameNotBeginException();
                }
                PlayerMeDto me = new(player.Role, player.CardsInHand);
                HashSet<OtherPlayerDto> otherPlayers = new();
                foreach (Player otherPlayer in room.Players.Where(p => p.Id != player.Id))
                {
                    otherPlayers.Add(new OtherPlayerDto(otherPlayer.Id, otherPlayer.CardsInHand.Count));
                }
                await Clients.Client(player.ConnectionId).PushGame(new GameDto(me, game, playersWithCardsDrawable, otherPlayers));
            }
        }
    }
}