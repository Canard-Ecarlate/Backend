using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class RoomIdNoExistException : Exception
    {
        private string? RoomId { get; set; }

        public RoomIdNoExistException() : 
            base("Room Id doesn't exist.")            
        {
        }

        public RoomIdNoExistException(string roomId) 
            : base($"Room Id {roomId} is not an userName.")
        {
            RoomId = roomId;
        }

        public RoomIdNoExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RoomIdNoExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}