namespace DuckCity.Tests.IntegrationTests.GameApi;

/**
 * Start
 * {"protocol":"json","version":1}
 *
 * CreateRoom
 * {"arguments":[{"RoomName":"","HostId":"","HostName":"","ContainerId":"","IsPrivate":true,"NbPlayers":5}],"invocationId":"0","target":"CreateRoom","type":1}
 *
 * JoinRoom
 * {"arguments":["your roomId","your userId","your userName"],"invocationId":"0","target":"JoinRoom","type":1}
 *
 * LeaveRoom
 * {"arguments":["your roomId","your userId"],"invocationId":"1","target":"LeaveRoom","type":1}
 *
 * PlayerReady
 * {"arguments":["your roomId"],"invocationId":"1","target":"PlayerReady","type":1}
 */
public class DuckCityHubIt
{
    
}