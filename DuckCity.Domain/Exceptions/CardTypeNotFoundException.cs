using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class CardTypeNotFoundException : Exception
    {
        public CardTypeNotFoundException() :
            base("Card type not found.")
        {
        }

        public CardTypeNotFoundException(string message)
            : base(message)
        {
        }

        public CardTypeNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CardTypeNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
