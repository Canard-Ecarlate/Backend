using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class GameNotBeginException : Exception
    {
        public GameNotBeginException() :
            base("Game is empty")
        {
        }

        public GameNotBeginException(string message) :
            base("Game " + message + " empty")
        {
        }

        public GameNotBeginException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected GameNotBeginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
