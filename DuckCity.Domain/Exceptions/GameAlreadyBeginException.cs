using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class GameAlreadyBeginException : Exception
    {
        public GameAlreadyBeginException() :
            base("Game already begin")
        {
        }

        public GameAlreadyBeginException(string message) :
            base("Game already begin : " + message)
        {
        }

        public GameAlreadyBeginException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected GameAlreadyBeginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
