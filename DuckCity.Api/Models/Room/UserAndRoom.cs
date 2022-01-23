using System.ComponentModel.DataAnnotations;

namespace DuckCity.Api.Models.Room
{
    public class UserAndRoom
    {
        [Required]
        public string UserId { get; init; } = "";
        [Required]
        public string UserName { get; init; } = "";
        [Required]
        public string RoomId { get; init; } = "";
    }
}
