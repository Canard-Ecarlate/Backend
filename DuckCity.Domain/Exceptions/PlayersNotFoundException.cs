using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class PlayersNotFoundException : Exception
    {
        public PlayersNotFoundException() : 
            base("Players not found in this room")            
        {
        }

        public PlayersNotFoundException(string message) :
            base(message)
        {
        }

        public PlayersNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PlayersNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}