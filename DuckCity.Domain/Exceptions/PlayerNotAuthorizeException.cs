using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class PlayerNotAuthorizeException : Exception
    {
        public PlayerNotAuthorizeException() :
            base("Player not authorize")
        {
        }

        public PlayerNotAuthorizeException(string message)
            : base(message)
        {
        }

        public PlayerNotAuthorizeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PlayerNotAuthorizeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
