using DuckCity.Domain.Cards;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Roles;
using DuckCity.Domain.Rooms;

namespace DuckCity.Domain.Games
{
    public class Game
    {
        public List<ICard> CardsInGame { get; set; } = new();
        public string CurrentPlayerId { get; set; }
        public string? PreviousPlayerId { get; set; }
        public ICard? PreviousDrawnCard { get; set; }
        public int RoundNb { get; set; }
        public int NbGreenDrawn { get; set; }
        public int NbDrawnDuringRound { get; set; }
        public int NbTotalRound { get; set; } = 4;
        public int NbCardsToDrawByRound { get; set; }
        public bool IsGameEnded { get; set; }
        public IRole? Winners { get; set; }

        public Game(string currentPlayerId, RoomConfiguration roomConfiguration)
        {
            CurrentPlayerId = currentPlayerId;
            PreviousPlayerId = null;
            PreviousDrawnCard = null;
            RoundNb = 0;
            NbGreenDrawn = 0;
            NbDrawnDuringRound = 0;
            IsGameEnded = false;
            NbCardsToDrawByRound = roomConfiguration.NbPlayers;
            Winners = null;
            foreach (NbEachCard nbEachCard in roomConfiguration.Cards)
            {
                Type? cardType = Type.GetType(nbEachCard.CardName + "Card");
                if (cardType != null)
                {
                    for (int i = 0; i < nbEachCard.Number; i++)
                    {
                        ICard? card = Activator.CreateInstance(cardType) as ICard;
                        if(card != null)
                        {
                            CardsInGame.Add(card);
                        }
                    }
                }
            }
            ShuffleCardsInGame();
        }

        /*
         * Draw a card in cardsInGame
         */
        public ICard DrawCard(Type typeDrawnCard)
        {
            ICard drawnCard = CardsInGame.First(card => card.GetType() == typeDrawnCard);
            if(drawnCard == null)
            {
                throw new CardNotFoundException();
            }

            return drawnCard;
        }

        /*
         * Update NbDrawnDuringRound, NbRound, PreviousPlayerId, PreviousDrawnCard
         */
        public void UpdateGameInfos(string playerWhoDrawId, ICard drawnCard)
        {
            PreviousPlayerId = playerWhoDrawId;
            PreviousDrawnCard = drawnCard;
            NbDrawnDuringRound++;
        }

        /*
         * Managing the endgame
         */
        public void EndGame()
        {
            IsGameEnded = true;
            if (NbGreenDrawn == NbCardsToDrawByRound)
            {
                Winners = new BlueRole();
            }
            else
            {
                Winners = new RedRole();
            }
        }

        /*
         * Shuffle cards and udate NbDrawnDuringRound
         */
        public void ShuffleCardsInGame()
        {
            if (CardsInGame != null && CardsInGame.Count % NbCardsToDrawByRound == 0)
            {
                NbDrawnDuringRound = 0;
                Random rnd = new();
                IOrderedEnumerable<ICard> shuffleCards = CardsInGame.OrderBy(card => rnd.Next());
                CardsInGame = new(shuffleCards);
            }
        }
    }
}