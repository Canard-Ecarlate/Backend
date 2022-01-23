using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException() : 
            base("Player not found in players")            
        {
        }

        public PlayerNotFoundException(string message) :
            base(message)
        {
        }

        public PlayerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PlayerNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}