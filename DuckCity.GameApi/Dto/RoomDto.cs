using DuckCity.Domain.Rooms;

namespace DuckCity.GameApi.Dto;

public class RoomDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public string HostId { get; set; } = "";
    public string HostName { get; set; } = "";
    public RoomConfiguration? RoomConfiguration { get; set; }
    public bool IsPlaying { get; set; }
}