using DuckCity.Application.Validations;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.RoomPreviewRepository;

namespace DuckCity.Application.RoomPreviewService;

public class RoomPreviewService : IRoomPreviewService
{
    private readonly IRoomPreviewRepository _roomPreviewRepository;

    public RoomPreviewService(IRoomPreviewRepository roomPreviewRepository)
    {
        _roomPreviewRepository = roomPreviewRepository;
    }

    public void CreateRoomPreview(RoomPreview newRoomPreview)
    {
        _roomPreviewRepository.Create(newRoomPreview);
    }

    public IEnumerable<RoomPreview> FindAllRooms() => _roomPreviewRepository.FindAllRooms();

    public RoomPreview FindRoom(string roomId)
    {
        CheckValid.IsObjectId(roomId);
        return _roomPreviewRepository.FindById(roomId);
    }

    public void UpdateRoomPreview(RoomPreview roomPreview)
    {
        _roomPreviewRepository.Update(roomPreview);
    }

    public void DeleteRoomPreview(string roomId)
    {
        _roomPreviewRepository.Delete(roomId);
    }
}