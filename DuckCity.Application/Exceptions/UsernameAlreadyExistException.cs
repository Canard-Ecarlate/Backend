using System.Runtime.Serialization;

namespace DuckCity.Application.Exceptions
{
    [Serializable]
    public class UsernameAlreadyExistException : Exception
    {
        private string? UserName { get; set; }
        
        public UsernameAlreadyExistException() : 
            base("Password and password confirmation are not equals.")            
        {
        }
        
        public UsernameAlreadyExistException(string? userName)
            : base($"Username {userName} already exist in our database.")
        {
            UserName = userName;
        }
        
        public UsernameAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UsernameAlreadyExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
