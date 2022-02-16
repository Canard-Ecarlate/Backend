using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class CardNotFoundException : Exception
    {
        public CardNotFoundException() :
            base("Card not found")
        {
        }

        public CardNotFoundException(string message) :
            base("Card " + message + " not found")
        {
        }

        public CardNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CardNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
