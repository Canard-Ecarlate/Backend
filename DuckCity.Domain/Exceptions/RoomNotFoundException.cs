using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException() : 
            base("Room not found in database")            
        {
        }

        public RoomNotFoundException(string message) :
            base("Room " + message + " does not exist")
        {
        }

        public RoomNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RoomNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}