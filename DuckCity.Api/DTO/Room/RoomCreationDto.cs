using System.ComponentModel.DataAnnotations;

namespace DuckCity.Api.DTO.Room
{
    public class RoomCreationDto
    {
        [Required]
        public string Name { get; init; } = "";
    }
}
