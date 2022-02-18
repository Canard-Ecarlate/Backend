using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class NotEnoughPlayersException : Exception
    {
        public NotEnoughPlayersException() :
            base("Not enough players")
        {
        }

        public NotEnoughPlayersException(string message) :
            base("Not enough players " + message)
        {
        }

        public NotEnoughPlayersException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotEnoughPlayersException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}