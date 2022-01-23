using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class HostIdNoExistException : Exception
    {
        private string? HostId { get; set; }

        public HostIdNoExistException() : 
            base("hostid doesn't exist.")            
        {
        }

        public HostIdNoExistException(string? hostId) 
            : base($"Hostid {hostId} does not exist in our database.")
        {
            HostId = hostId;
        }

        public HostIdNoExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected HostIdNoExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}