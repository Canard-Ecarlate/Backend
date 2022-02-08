using DuckCity.Application.Validations;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.RoomPreviewRepository;
using DuckCity.Infrastructure.UserRepository;

namespace DuckCity.Application.RoomPreviewService;

public class RoomPreviewService : IRoomPreviewService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomPreviewRepository _roomPreviewRepository;

    public RoomPreviewService(IRoomPreviewRepository roomPreviewRepository, IUserRepository userRepository)
    {
        _roomPreviewRepository = roomPreviewRepository;
        _userRepository = userRepository;
    }
    
    public RoomPreview CreateAndJoinRoomPreview(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers)
    {
        CheckValid.CreateRoom(_roomPreviewRepository, _userRepository, roomName, hostId);
        
        RoomPreview roomPreview = new(roomName, hostId, hostName, isPrivate, nbPlayers);
        _roomPreviewRepository.Create(roomPreview);
        
        return roomPreview;
    }
    
    public RoomPreview JoinRoomPreview(string roomId, string userId)
    {
        CheckValid.JoinRoom(_roomPreviewRepository, _userRepository, userId);
        RoomPreview roomPreview = _roomPreviewRepository.FindById(roomId);
        roomPreview.PlayersId.Add(userId);
        _roomPreviewRepository.Update(roomPreview);
        return roomPreview;
    }

    public RoomPreview? LeaveRoomPreview(string roomId, string userId)
    {
        RoomPreview roomPreview = _roomPreviewRepository.FindById(roomId);
        CheckValid.LeaveRoom(_userRepository, userId, roomPreview);

        roomPreview.PlayersId.Remove(userId);
        if (roomPreview.PlayersId is {Count: 0})
        {
            _roomPreviewRepository.Delete(roomPreview);
            return null;
        }
        _roomPreviewRepository.Update(roomPreview);
        return roomPreview;
    }
    
    public RoomPreview FindRoom(string roomId)
    {
        CheckValid.IsObjectId(roomId);
        return _roomPreviewRepository.FindById(roomId);
    }
    
    public IEnumerable<RoomPreview> FindAllRooms() => _roomPreviewRepository.FindAllRooms();
}