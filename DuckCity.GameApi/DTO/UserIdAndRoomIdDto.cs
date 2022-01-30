using System.ComponentModel.DataAnnotations;

namespace DuckCity.GameApi.DTO
{
    public class UserIdAndRoomIdDto
    {
        [Required] 
        public string UserId { get; init; } = "";
        [Required]
        public string RoomId { get; init; } = "";
    }
}
