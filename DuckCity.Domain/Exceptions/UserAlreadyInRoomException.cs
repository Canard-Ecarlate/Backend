using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class UserAlreadyInRoomException : Exception
    {
        private string? UserId { get; set; }

        public UserAlreadyInRoomException() : 
            base("User already in other room.")            
        {
        }

        public UserAlreadyInRoomException(string? userId) 
            : base($"User with ID =  {userId} already in other room.")
        {
            UserId = userId;
        }

        public UserAlreadyInRoomException(string? message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UserAlreadyInRoomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}