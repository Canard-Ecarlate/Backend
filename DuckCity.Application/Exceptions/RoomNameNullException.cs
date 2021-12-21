using System.Runtime.Serialization;

namespace DuckCity.Application.Exceptions
{
    [Serializable]
    public class RoomNameNullException : Exception
    {
        public RoomNameNullException() : 
            base("RoomName cannot be null.")            
        {
        }

        public RoomNameNullException(string message) :
            base(message)
        {
        }

        public RoomNameNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RoomNameNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}