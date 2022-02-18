using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class CardsInGameEmptyException : Exception
    {
        public CardsInGameEmptyException() :
            base("Cards in game empty")
        {
        }

        public CardsInGameEmptyException(string message) :
            base("Cards in game " + message + " empty")
        {
        }

        public CardsInGameEmptyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CardsInGameEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
