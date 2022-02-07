using DuckCity.Application.Services.Interfaces;
using DuckCity.Application.Validations;
using DuckCity.Domain.Games;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Repositories;

namespace DuckCity.Application.Services;

public class RoomService : IRoomService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IGameRepository _gameRepository;

    public RoomService(IUserRepository userRepository, IRoomRepository roomRepository, IGameRepository gameRepository)
    {
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _gameRepository = gameRepository;
    }

    public IEnumerable<Room> FindAllRooms() => _roomRepository.FindAllRooms();

    public Room FindRoom(string roomId)
    {
        CheckValid.IsObjectId(roomId);
        return _roomRepository.FindById(roomId);
    } 

    public Room CreateAndJoinRoom(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers)
    {
        CheckValid.CreateRoom(_roomRepository, _userRepository, roomName, hostId);
        
        Room room = new(roomName, hostId, hostName, isPrivate, nbPlayers);
        _roomRepository.Create(room);
        
        return room;
    }

    public Room JoinRoom(string roomId, string userId, string userName)
    {
        CheckValid.JoinRoom(_roomRepository, _userRepository, userId);
        Room room = _roomRepository.JoinRoom(roomId, userId);
        return room;
    }
    
    public Game JoinGameAndConnect(string connectionId, string userId, string userName, string roomId)
    {
        Game? game = _gameRepository.FindGameByRoomId(roomId);
        if (game == null)
        {
            Game newGame = new(roomId, connectionId, userId, userName);
            _gameRepository.Add(newGame);
            return newGame;
        }
        Player? player = game.Players.SingleOrDefault(p => p.Id == userId);
        if (player == null)
        {
            Player newPlayer = new(connectionId, userId, userName);
            game.Players.Add(newPlayer);
        }
        else
        {
            player.ConnectionId = connectionId;
        }

        return game;
    }

    public Game? LeaveGameAndDisconnect(string roomId, string connectionId)
    {
        Game game = _gameRepository.FindGameByRoomId(roomId)!;
        Player player = game.Players.Single(p => p.ConnectionId == connectionId);
        Room room = _roomRepository.FindById(roomId);
        CheckValid.LeaveRoom(_userRepository, player.Id, room);
        
        // Remove player in cache
        game.Players.Remove(player); 

        // Remove player in database
        room.PlayersId.Remove(player.Id);
        if (room.PlayersId is {Count: 0})
        {
            _gameRepository.Remove(game);
            _roomRepository.Delete(room);
            return null;
        }
        _roomRepository.Replace(room);
        return game;
    }
    
    public Game PlayerReady(string roomId, string connectionId)
    {
        return _gameRepository.SetPlayerReadyInGame(roomId, connectionId);
    }

    public Game? DisConnectToRoom(string connectionId)
    {
        return _gameRepository.DisconnectPlayerFromGame(connectionId);
    }
}