using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class CardsInGameNumberNotMatchPlayersNumber : Exception
    {
        public CardsInGameNumberNotMatchPlayersNumber() :
            base("Cards in game number doesn't match players number")
        {
        }

        public CardsInGameNumberNotMatchPlayersNumber(string message) :
            base("Cards in game number " + message + " doesn't match players number")
        {
        }

        public CardsInGameNumberNotMatchPlayersNumber(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CardsInGameNumberNotMatchPlayersNumber(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
