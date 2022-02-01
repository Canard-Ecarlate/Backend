namespace DuckCity.GameApi.Hub;

public class SignalRUser
{
    public string ConnectionId { get; init; } = "";
    public string UserId { get; init; } = "";
    public string RoomId { get; init; } = "";

    public override bool Equals(object? obj)
    {
        return obj is SignalRUser signalRUser && ConnectionId == signalRUser.ConnectionId
                                              && UserId == signalRUser.UserId
                                              && RoomId == signalRUser.RoomId;
    }

    public override int GetHashCode()
    {
        return new {ConnectionId, UserId, RoomId}.GetHashCode();
    }
}