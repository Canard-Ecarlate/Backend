﻿using DuckCity.Domain.Cards;
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
        public Room? StartGame(string roomId, RoomConfiguration roomConfiguration)
        {
            Room? room = _roomRepository.FindById(roomId);
            if(room != null)
            {
                // Todo Checkvalid assez de joueurs et tous prêts
                HashSet<Player> players = room.Players;

                // Designate first player
                Random random = new();
                HashSet<Player> playerToShuffle = new(players.OrderBy(player => random.Next()));
                Player firstPlayer = playerToShuffle.First();
                room.Game = new Game(firstPlayer.Id, roomConfiguration);
                firstPlayer.IsCardsDrawable = false;

                // Init the first round and assign cards to players
                NewRound(players, room.Game);

                // Assign roles to players
                AssignRole(playerToShuffle, roomConfiguration.Roles);
                return room;
            }
            return null;

        }

        /*
         * Assign randomly roles to players
         */
        private static void AssignRole(HashSet<Player> players, List<NbEachRole> roles)
        {
            List<IRole> rolesInGame = new();
            foreach (NbEachRole nbEachRole in roles)
            {
                Type? roleType = Type.GetType(nbEachRole.RoleName + "Role");
                if (roleType != null)
                {
                    for (int i = 0; i < nbEachRole.Number; i++)
                    {
                        IRole? role = Activator.CreateInstance(roleType) as IRole;
                        if (role != null)
                        {
                            rolesInGame.Add(role);
                        }
                    }
                }
            }
            Random random = new();
            rolesInGame = new(rolesInGame.OrderBy(role => random.Next()));
            foreach(Player player in players)
            {
                player.Role = rolesInGame.First();
                rolesInGame.Remove(player.Role);
            }
        }

        /*
         * Stop the game without winners
         */
        public void QuitMidGame(string roomId, string playerWhoQuitsId) // Todo
        {
            Room? room = _roomRepository.FindById(roomId);
            if (room != null)
            {
                Game? game = room.Game;
                if (game != null)
                {
                    game.IsGameEnded = true;
                }
            }
        }

        /*
         * Draw randomly a card in player hands
         */
        public Room? DrawCard(string playerWhoDrawId, string playerWhereCardIsDrawingId, string roomId)
        {
            Room? room = _roomRepository.FindById(roomId);
            if(room == null)
            {
                return null;
            }

            Game? game = room.Game;
            HashSet<Player> players = room.Players;
            if (game == null)
            {
                return null;
            }

            // get player who is drawing and from whom
            Player playerWhereCardIsDrawing = players.First(player => player.Id == playerWhereCardIsDrawingId);
            Player playerWhoDraw = players.First(player => player.Id == playerWhoDrawId);
            if (playerWhereCardIsDrawing == null || playerWhoDraw == null)
            {
                return null;
            }

            // randomly draw a card in CardsInHand from playerWhereCardIsDrawing
            Random random = new Random();
            int nbCardsInHand = playerWhereCardIsDrawing.CardsInHand.Count;
            if(nbCardsInHand == 0)
            {
                return null;
            }
            Type typeDrawnCard = playerWhereCardIsDrawing.CardsInHand.ElementAt(random.Next(nbCardsInHand)).GetType();
            playerWhereCardIsDrawing.CardsInHand.RemoveAt(nbCardsInHand);

            ICard? drawnCard = game.DrawCard(typeDrawnCard);
            if (drawnCard == null)
            {
                return null;
            }

            // Card action
            drawnCard.DrawAction(playerWhereCardIsDrawing, playerWhoDraw, game, players);
            game.CardsInGame.Remove(drawnCard);
            UpdateGameInfos(playerWhoDrawId, drawnCard, players, game);

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
            if (game.CardsInGame != null && game.CardsInGame.Count % players.Count == 0)
            {
                game.ShuffleCardsInGame();
                int nbCardsInHand = game.CardsInGame.Count / players.Count;
                Random random = new Random();
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
}