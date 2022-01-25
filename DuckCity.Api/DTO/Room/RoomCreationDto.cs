using System.ComponentModel.DataAnnotations;

namespace DuckCity.Api.DTO.Room
{
    public class RoomCreationDto
    {
        [Required]
        public string Name { get; init; } = "";
        [Required]
        public string HostId { get; init; } = "";
        [Required]
        public string HostName { get; init; } = "";
        [Required]
        public bool IsPrivate { get; init; }
        [Required]
        public int NbPlayers { get; init; }
    }
}
