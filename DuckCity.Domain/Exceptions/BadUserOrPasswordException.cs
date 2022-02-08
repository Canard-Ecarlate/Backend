using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class BadUserOrPasswordException : Exception
    {
        public BadUserOrPasswordException() : 
            base("User or Password are not correct.")            
        {
        }

        public BadUserOrPasswordException(string message)
            :base(message)
        {
        }

        public BadUserOrPasswordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BadUserOrPasswordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
