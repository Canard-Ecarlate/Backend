using System.Runtime.Serialization;

namespace DuckCity.Application.Exceptions
{
    [Serializable]
    public class RoomNameAlreadyExistException : Exception
    {
        private string? RoomName { get; set; }

        public RoomNameAlreadyExistException() : 
            base("room name already exists")            
        {
        }

        public RoomNameAlreadyExistException(string roomName) 
            : base($"room name {roomName} already exists.")
        {
            RoomName = roomName;
        }

        public RoomNameAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RoomNameAlreadyExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}