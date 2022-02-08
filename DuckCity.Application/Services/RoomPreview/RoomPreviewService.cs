using DuckCity.Application.Validations;
using DuckCity.Infrastructure.Repositories;
using DuckCity.Infrastructure.Repositories.RoomPreview;
using DuckCity.Infrastructure.Repositories.User;

namespace DuckCity.Application.Services.RoomPreview;

public class RoomPreviewService : IRoomPreviewService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomPreviewRepository _roomPreviewRepository;

    public RoomPreviewService(IRoomPreviewRepository roomPreviewRepository, IUserRepository userRepository)
    {
        _roomPreviewRepository = roomPreviewRepository;
        _userRepository = userRepository;
    }
    
    public Domain.Rooms.RoomPreview CreateAndJoinRoomPreview(string roomName, string hostId, string hostName, bool isPrivate, int nbPlayers)
    {
        CheckValid.CreateRoom(_roomPreviewRepository, _userRepository, roomName, hostId);
        
        Domain.Rooms.RoomPreview roomPreview = new(roomName, hostId, hostName, isPrivate, nbPlayers);
        _roomPreviewRepository.Create(roomPreview);
        
        return roomPreview;
    }
    
    public Domain.Rooms.RoomPreview JoinRoomPreview(string roomId, string userId)
    {
        CheckValid.JoinRoom(_roomPreviewRepository, _userRepository, userId);
        Domain.Rooms.RoomPreview roomPreview = _roomPreviewRepository.FindById(roomId);
        roomPreview.PlayersId.Add(userId);
        _roomPreviewRepository.Replace(roomPreview);
        return roomPreview;
    }

    public Domain.Rooms.RoomPreview? LeaveRoomPreview(string roomId, string userId)
    {
        Domain.Rooms.RoomPreview roomPreview = _roomPreviewRepository.FindById(roomId);
        CheckValid.LeaveRoom(_userRepository, userId, roomPreview);

        roomPreview.PlayersId.Remove(userId);
        if (roomPreview.PlayersId is {Count: 0})
        {
            _roomPreviewRepository.Delete(roomPreview);
            return null;
        }
        _roomPreviewRepository.Replace(roomPreview);
        return roomPreview;
    }
    
    public Domain.Rooms.RoomPreview FindRoom(string roomId)
    {
        CheckValid.IsObjectId(roomId);
        return _roomPreviewRepository.FindById(roomId);
    }
    
    public IEnumerable<Domain.Rooms.RoomPreview> FindAllRooms() => _roomPreviewRepository.FindAllRooms();
}