using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class PlayerNotReadyException : Exception
    {
        public PlayerNotReadyException() :
            base("Player not ready")
        {
        }

        public PlayerNotReadyException(string message) :
            base("Player " + message + " not ready")
        {
        }

        public PlayerNotReadyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PlayerNotReadyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}