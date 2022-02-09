using System.ComponentModel.DataAnnotations;

namespace DuckCity.Api.DTO.Room
{
    public class UserAndRoomDto
    {
        [Required]
        public string UserId { get; init; } = "";
        [Required]
        public string RoomId { get; init; } = "";
    }
}
