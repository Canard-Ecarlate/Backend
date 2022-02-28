namespace DuckCity.Tests.IntegrationTests.GameApi;

/**
 * Start
 * {"protocol":"json","version":1}
 *
 * CreateRoom
 * {"arguments":[{"RoomName":"","HostId":"","HostName":"","ContainerId":"","IsPrivate":true,"NbPlayers":5}],"invocationId":"0","target":"CreateRoom","type":1}
 *
 * JoinRoom
 * {"arguments":["your roomCode"],"invocationId":"0","target":"JoinRoom","type":1}
 *
 * LeaveRoom
 * {"arguments":["your roomCode"],"invocationId":"1","target":"LeaveRoom","type":1}
 *
 * PlayerReady
 * {"arguments":["your roomCode"],"invocationId":"1","target":"PlayerReady","type":1}
 * 
 * StartGame
 * {"arguments":["your roomCode"],"invocationId":"1","target":"StartGame","type":1}
 * 
 * DrawCard
 * {"arguments":["your roomCode","user id where draw"],"invocationId":"1","target":"DrawCard","type":1}
 * 
 * QuitMidGame
 * {"arguments":["your roomCode"],"invocationId":"1","target":"QuitMidGame","type":1}
 */
public class DuckCityHubIt
{
    
}