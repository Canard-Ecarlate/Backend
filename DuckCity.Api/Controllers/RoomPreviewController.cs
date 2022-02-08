using DuckCity.Api.DTO.Room;
using DuckCity.Application.Services.RoomPreview;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class RoomPreviewController : ControllerBase
{
    private readonly IRoomPreviewService _roomPreviewService;

    public RoomPreviewController(IRoomPreviewService roomPreviewService)
    {
        _roomPreviewService = roomPreviewService;
    }
    
    [HttpPost]
    public ActionResult<RoomPreview> CreateAndJoinRoom(RoomCreationDto room)
    {
        RoomPreview roomPreviewCreated = _roomPreviewService.CreateAndJoinRoomPreview(room.Name, room.HostId, room.HostName, room.IsPrivate, room.NbPlayers);
        return new OkObjectResult(roomPreviewCreated);
    }
        
    [HttpPost]
    public ActionResult<RoomPreview> JoinRoom(UserAndRoomDto userAndRoomDto)
    {
        RoomPreview roomPreviewJoined = _roomPreviewService.JoinRoomPreview(userAndRoomDto.RoomId, userAndRoomDto.UserId);
        return new OkObjectResult(roomPreviewJoined);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<RoomPreview>> FindAllRooms() => new OkObjectResult(_roomPreviewService.FindAllRooms());
}