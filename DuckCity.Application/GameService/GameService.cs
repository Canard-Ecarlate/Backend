using DuckCity.Application.Validations;
using DuckCity.Domain.Cards;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Games;
using DuckCity.Domain.Roles;
using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.RoomRepository;

namespace DuckCity.Application.GameService
{
    public class GameService : IGameService
    {
        private readonly IRoomRepository _roomRepository;

        public GameService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        /*
         * Init the game
         * Assign role to player
         * Assign cards to player
         * Designate first player
         */
        public Room StartGame(string roomCode)
        {
            Room? room = _roomRepository.FindByCode(roomCode);
            if (room == null)
            {
                throw new RoomNotFoundException();
            }
            CheckValid.StartGame(_roomRepository, room);
            HashSet<Player> players = room.Players;

            // Designate first player
            Random random = new();
            HashSet<Player> playerToShuffle = new(players.OrderBy(player => random.Next()));
            Player firstPlayer = playerToShuffle.First();
            room.Game = new Game(firstPlayer.Id, room.RoomConfiguration);
            firstPlayer.IsCardsDrawable = false;

            // Init the first round and assign cards to players
            NewRound(players, room.Game);

            // Assign roles to players
            AssignRole(playerToShuffle, room.RoomConfiguration.Roles);
            _roomRepository.Update(room);
            return room;
        }

        /*
         * Assign randomly roles to players
         */
        private static void AssignRole(HashSet<Player> players, List<NbEachRole> roles)
        {
            List<string> rolesInGame = new();
            foreach (NbEachRole nbEachRole in roles)
            {
                for (int i = 0; i < nbEachRole.Number; i++)
                {
                    rolesInGame.Add(nbEachRole.RoleName);
                }
            }
            Random random = new();
            rolesInGame = new(rolesInGame.OrderBy(role => random.Next()));
            foreach (Player player in players)
            {
                string roleName = rolesInGame.First();
                player.AssignRole(roleName);
                rolesInGame.Remove(roleName);
            }
        }

        /*
         * Stop the game without winners
         */
        public void QuitMidGame(string roomCode)
        {
            Room? room = _roomRepository.FindByCode(roomCode);
            if (room == null)
            {
                throw new RoomNotFoundException();
            }
            if (room.Game == null)
            {
                throw new GameNotBeginException();
            }
            room.Game.IsGameEnded = true;
        }

        /*
         * Draw randomly a card in player hands
         */
        public Room DrawCard(string playerWhoDrawId, string playerWhereCardIsDrawingId, string roomCode)
        {
            Room? room = _roomRepository.FindByCode(roomCode);
            if (room == null)
            {
                throw new RoomNotFoundException();
            }

            Game? game = room.Game;
            HashSet<Player> players = room.Players;
            if (game == null)
            {
                throw new GameNotBeginException();
            }

            // get player who is drawing and from whom
            Player playerWhereCardIsDrawing = players.First(player => player.Id == playerWhereCardIsDrawingId);
            Player playerWhoDraw = players.First(player => player.Id == playerWhoDrawId);
            if (playerWhereCardIsDrawing == null || playerWhoDraw == null)
            {
                throw new PlayerNotFoundException();
            }

            // randomly draw a card in CardsInHand from playerWhereCardIsDrawing
            Type typeDrawnCard = playerWhereCardIsDrawing.DrawCard();

            ICard drawnCard = game.DrawCard(typeDrawnCard);

            // Card action
            drawnCard.DrawAction(playerWhereCardIsDrawing, playerWhoDraw, game, players);
            game.CardsInGame.Remove(drawnCard);
            UpdateGameInfos(playerWhoDrawId, drawnCard, players, game);

            _roomRepository.Update(room);
            return room;
        }

        /*
         * Update NbDrawnDuringRound, NbRound, PreviousPlayerId, PreviousDrawnCard
         */
        private static void UpdateGameInfos(string playerWhoDrawId, ICard drawnCard, HashSet<Player> players, Game game)
        {
            game.UpdateGameInfos(playerWhoDrawId, drawnCard);
            if (game.NbDrawnDuringRound > players.Count)
            {
                game.RoundNb++;
                if (game.RoundNb > game.NbTotalRound)
                {
                    game.EndGame();
                }
                else
                {
                    NewRound(players, game);
                }
            }
        }

        /*
         * Makes a new round
         */
        private static void NewRound(HashSet<Player> players, Game game)
        {
            if (game.CardsInGame == null)
            {
                throw new CardsInGameEmptyException();
            }
            if (game.CardsInGame.Count % players.Count != 0)
            {
                throw new CardsInGameNumberNotMatchPlayersNumber();
            }
            game.ShuffleCardsInGame();
            int nbCardsInHand = game.CardsInGame.Count / players.Count;
            Random random = new();
            HashSet<Player> playerToShuffle = new(players.OrderBy(player => random.Next()));
            int start = 0;
            int end = start + nbCardsInHand;
            foreach (Player shufflePlayer in playerToShuffle)
            {
                Player player = players.First(player => player.Id == shufflePlayer.Id);
                player.CardsInHand = new(game.CardsInGame.Take(new Range(start, end)));
                start = end;
                end += nbCardsInHand;
            }
        }
    }
}