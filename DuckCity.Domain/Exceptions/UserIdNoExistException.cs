using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class UserIdNoExistException : Exception
    {
        private string? UserId { get; set; }

        public UserIdNoExistException() : 
            base("User Id doesn't exist.")            
        {
        }

        public UserIdNoExistException(string? userId) 
            : base($"The User Id {userId} does not exist in our database.")
        {
            UserId = userId;
        }

        public UserIdNoExistException(string? message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UserIdNoExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}