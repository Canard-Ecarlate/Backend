using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class CodeRoomNotExistException : Exception
    {
        public CodeRoomNotExistException() : 
            base("Code of the room not exist")            
        {
        }

        public CodeRoomNotExistException(string code) :
            base("CodeRoom " + code + " does not exist")
        {
        }

        public CodeRoomNotExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CodeRoomNotExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
