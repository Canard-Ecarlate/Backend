using DuckCity.Application.RoomPreviewService;
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
    
    [HttpGet]
    public ActionResult<IEnumerable<RoomPreview>> FindAllRooms() =>
        new OkObjectResult(_roomPreviewService.FindAllRooms());
}