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
    
    public string GenerateCode()
    {
        const int lenghtCode = 4;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string code = new(Enumerable.Repeat(chars, lenghtCode).Select(s => s[Random.Next(s.Length)]).ToArray());
        return !CodeIsValid(code) ? GenerateCode() : code;
    }

    private bool CodeIsValid(string code)
    {
        if (_forbiddenWords.Contains(code))
        {
            return false;
        }
        return !_roomPreviewRepository.CodeIsExist(code);
    }

    public IEnumerable<RoomPreview> FindAllRooms() => _roomPreviewRepository.FindAllRooms();

    public RoomPreview? FindByUserId(string userId)
    {
        return _roomPreviewRepository.FindByUserId(userId);
    }

    public void UpdateRoomPreview(RoomPreview roomPreview)
    {
        _roomPreviewRepository.Update(roomPreview);
    }

    public void DeleteRoomPreview(string roomCode)
    {
        _roomPreviewRepository.Delete(roomCode);
    }

    public void DeleteRoomPreviewByUserId(string userId)
    {
        RoomPreview? roomPreviewNoExists = _roomPreviewRepository.FindByUserId(userId);
        if (roomPreviewNoExists != null)
        {
            _roomPreviewRepository.Delete(roomPreviewNoExists.Code);
        }

    }
}