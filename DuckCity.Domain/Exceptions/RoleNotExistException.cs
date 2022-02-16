using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    public class RoleNotExistException : Exception
    {
        public RoleNotExistException() :
            base("Cards in game empty")
        {
        }

        public RoleNotExistException(string message) :
            base("Cards in game " + message + " empty")
        {
        }

        public RoleNotExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RoleNotExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
