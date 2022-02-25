using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class PasswordIsNullException : Exception
    {
        public PasswordIsNullException() : 
            base("Password cannot be null.")            
        {
        }

        public PasswordIsNullException(string message)
            :base(message)
        {
        }

        public PasswordIsNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PasswordIsNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
