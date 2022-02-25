using System.ComponentModel.DataAnnotations;

namespace DuckCity.Api.DTO.Room
{
    public class UserAndRoomDto
    {
        
        [Required]
        public string RoomCode { get; init; } = "";
    }
}
