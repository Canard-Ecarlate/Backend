using DuckCity.Application.Validations;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.RoomPreviewRepository;

namespace DuckCity.Application.RoomPreviewService;

public class RoomPreviewService : IRoomPreviewService
{
    private readonly IRoomPreviewRepository _roomPreviewRepository;
    private static readonly Random Random = new();
    private readonly string[] _forbiddenWords = {"nazi", "nazy", "pute", "slut"};

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
    
    public string GenerateCode()
    {
        const int lenghtCode = 4;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string code = new string(Enumerable.Repeat(chars, lenghtCode).Select(s => s[Random.Next(s.Length)]).ToArray());
        if (!CodeIsValid(code))
        {
            return GenerateCode();
        }
        return code;
    }

    private bool CodeIsValid(string code)
    {
        if (_forbiddenWords.Contains(code))
        {
            return false;
        }
        return !_roomPreviewRepository.CodeIsExist(code);
    }
}