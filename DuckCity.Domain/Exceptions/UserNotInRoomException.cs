using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class UserNotInRoomException : Exception
    {
        private string? UserId { get; set; }

        public UserNotInRoomException() : 
            base("User not in room.")            
        {
        }

        public UserNotInRoomException(string? userId) 
            : base($"User with ID =  {userId} not in room.")
        {
            UserId = userId;
        }

        public UserNotInRoomException(string? message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UserNotInRoomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}