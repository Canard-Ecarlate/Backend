using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class GameFinishedException : Exception
    {
        public GameFinishedException() :
            base("User or Password are not correct.")
        {
        }

        public GameFinishedException(string message)
            : base(message)
        {
        }

        public GameFinishedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected GameFinishedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
