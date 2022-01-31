namespace DuckCity.Domain.Rooms;

public class PlayerInRoom
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public bool Ready { get; set; }

    public bool Connected { get; set; }
    
    public override bool Equals(object? obj)
    {
        return obj is PlayerInRoom playerInRoom && Id == playerInRoom.Id;
    }

    public override int GetHashCode()
    {
        return new {Id}.GetHashCode();
    }
}
