using System.ComponentModel.DataAnnotations;

namespace DuckCity.GameApi.Models
{
    public class UserAndRoom
    {
        [Required] public string UserId { get; init; } = "";
        [Required]
        public string RoomId { get; init; } = "";
    }
}
