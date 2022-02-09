namespace DuckCity.GameApi.Dto;

public class RoomCreationDto
{
    public string RoomName { get; set; } = "";
    public string HostId { get; set; } = "";
    public string HostName { get; set; } = "";
    public string ContainerId { get; set; } = "";
    public bool IsPrivate { get; set; }
    public int NbPlayers { get; set; }
}