﻿using DuckCity.Domain.Games;
using DuckCity.Domain.Rooms;

namespace DuckCity.Application.GameService
{
    public interface IGameService
    {
        Room StartGame(string roomId);
        void QuitMidGame(string roomId, string playerWhoQuits);
        Room? DrawCard(string playerWhoDrawId, string playerWhereCardIsDrawingId, string roomId);
    }
}