using DuckCity.Domain.Games;
using DuckCity.Domain.Rooms;

namespace DuckCity.Application.GameService
{
    public interface IGameService
    {
        Room StartGame(string roomCode);
        void QuitMidGame(string roomCode);
        Room DrawCard(string playerWhoDrawId, string playerWhereCardIsDrawingId, string roomCode);
    }
}