using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    internal class CardsInHandEmptyException : Exception
    {
        public CardsInHandEmptyException() :
            base("Cards in hand empty")
        {
        }

        public CardsInHandEmptyException(string message) :
            base("Cards in hand " + message + " empty")
        {
        }

        public CardsInHandEmptyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CardsInHandEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
