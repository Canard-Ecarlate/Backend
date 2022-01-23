using DuckCity.Api.Models.Room;
using DuckCity.Application.Services;
using DuckCity.Domain.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost]
        public Task<ActionResult<string>> CreateRoom(RoomCreation room)
        {
            Room roomCreated = _roomService.AddRooms(room.Name, room.HostId, room.HostName, room.IsPrivate, room.NbPlayers);
            return JoinRoom(new UserAndRoom {UserId = room.HostId, UserName = room.HostName, RoomId = roomCreated.Id});
        }
        
        [HttpPost]
        public async Task<ActionResult<string>> JoinRoom(UserAndRoom userAndRoom)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            
            JsonContent json = JsonContent.Create(userAndRoom);
            Task<HttpResponseMessage> stringTask = client.PostAsync("https://localhost:7143/api/Room/JoinRoom", json);
            HttpResponseMessage msg = await stringTask;
            Console.Write(msg.Content);
            
            return new OkObjectResult("ok");
        }
    }
}