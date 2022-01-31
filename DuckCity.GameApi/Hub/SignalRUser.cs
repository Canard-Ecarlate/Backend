namespace DuckCity.GameApi.Hub;

public class SignalRUser
{
    public string ContextId { get; init; } = "";
    public string UserId { get; init; } = "";

    public override bool Equals(object? obj)
    {
        return obj is SignalRUser signalRUser && ContextId == signalRUser.ContextId;
    }

    public override int GetHashCode()
    {
        return new {ContextId}.GetHashCode();
    }
}