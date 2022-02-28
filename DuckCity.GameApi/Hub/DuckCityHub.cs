using AutoMapper;
using DuckCity.Application.GameService;
using DuckCity.Application.RoomPreviewService;
using DuckCity.Application.RoomService;
using DuckCity.Application.Utils;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.GameApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DuckCity.GameApi.Hub;

[Authorize]
public class DuckCityHub : Hub<IDuckCityClient>
{
    // symbole � mettre � la fin des requ�te sur websocket king : 
    private readonly IRoomService _roomService;
    private readonly IGameService _gameService;
    private readonly IRoomPreviewService _roomPreviewService;
    private readonly IMapper _mapper;

    // Constructor
    public DuckCityHub(IRoomService roomService, IGameService gameService, IMapper mapper,
        IRoomPreviewService roomPreviewService)
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
        string userId = UserUtils.GetPayloadFromToken(Context.GetHttpContext(), "userId");
        bool userAlreadyInRoom = _roomService.UserIsInRoom(userId);

        if (userAlreadyInRoom)
        {
            // Set ConnectionId
            Room room = _roomService.ReconnectRoom(Context.ConnectionId, userId);

            // Join SignalR
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Code);

            // Send
            await Clients.Caller.PushRoom(_mapper.Map<RoomDto>(room));
            if (room.Game != null)
            {
                await SendGameInfo(room,
                    room.Players.Single(p => p.Id == userId && p.ConnectionId == Context.ConnectionId));
            }
            else
            {
                await Clients.Group(room.Code)
                    .PushPlayers(_mapper.Map<IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
            }
        }
        else
        {
            // delete roomPreview if exist without real room
            _roomPreviewService.DeleteRoomPreviewByUserId(userId);
        }
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
        string userId = UserUtils.GetPayloadFromToken(Context.GetHttpContext(), "userId");
        string userName = UserUtils.GetPayloadFromToken(Context.GetHttpContext(), "userName");

        Room newRoom = new(roomDto.RoomName, userId, userName, roomDto.ContainerId, roomDto.IsPrivate,
            roomDto.NbPlayers, Context.ConnectionId, _roomPreviewService.GenerateCode());

        // Create Room
        _roomService.CreateRoom(newRoom);

        // Create RoomPreview from Room
        _roomPreviewService.CreateRoomPreview(new RoomPreview(newRoom));

        // Join SignalR
        await Groups.AddToGroupAsync(Context.ConnectionId, newRoom.Code);

        // Send
        await Clients.Caller.PushRoom(_mapper.Map<RoomDto>(newRoom));
        await Clients.Group(newRoom.Code)
            .PushPlayers(_mapper.Map<IEnumerable<PlayerInWaitingRoomDto>>(newRoom.Players));
    }


    [HubMethodName("JoinRoom")]
    public async Task JoinRoomAsync(string roomCode)
    {
        string userId = UserUtils.GetPayloadFromToken(Context.GetHttpContext(), "userId");
        string userName = UserUtils.GetPayloadFromToken(Context.GetHttpContext(), "userName");

        // Join Room
        Room room = _roomService.JoinRoom(Context.ConnectionId, userId, userName, roomCode);

        // Update RoomPreview from Room
        _roomPreviewService.UpdateRoomPreview(new RoomPreview(room));

        // Join SignalR
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

        // Send
        await Clients.Caller.PushRoom(_mapper.Map<RoomDto>(room));
        await Clients.Group(roomCode).PushPlayers(_mapper.Map<IEnumerable<PlayerInWaitingRoomDto>>(room.Players));
    }

    [HubMethodName("LeaveRoom")]
    public async Task LeaveRoomAsync(string roomCode)
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
    public async Task StartGameAsync(string roomCode)
    {
        Room room = _gameService.StartGame(roomCode);
        await SendGameInfoAllPlayers(room);
    }

    [HubMethodName("DrawCard")]
    public async Task DrawCardAsync(string roomCode, string playerWhereCardIsDrawingId)
    {
        Room room = _gameService.DrawCard(Context.ConnectionId, playerWhereCardIsDrawingId, roomCode);
        await SendGameInfoAllPlayers(room);
    }

    [HubMethodName("QuitMidGame")]
    public async Task QuitMidGameAsync(string roomCode)
    {
        Room room = _gameService.QuitMidGame(roomCode);
        await SendGameInfoAllPlayers(room);

        await LeaveRoomAsync(roomCode);
    }

    private async Task SendGameInfoAllPlayers(Room room)
    {
        if (room.Game == null)
        {
            throw new GameNotBeginException();
        }

        foreach (Player player in room.Players)
        {
            await SendGameInfo(room, player);
        }
    }

    private async Task SendGameInfo(Room room, Player player)
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

            HashSet<string> playersWithCardsDrawable =
                new(room.Players.Where(p => p.IsCardsDrawable).Select(p => p.Id));
            await Clients.Client(player.ConnectionId)
                .PushGame(new GameDto(me, room.Game!, playersWithCardsDrawable, otherPlayers));
        }
    }
}