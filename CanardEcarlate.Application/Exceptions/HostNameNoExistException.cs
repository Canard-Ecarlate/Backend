using System;
using System.Runtime.Serialization;

namespace CanardEcarlate.Application.Exceptions
{
    [Serializable]
    public class HostNameNoExistException : Exception
    {
        private string Hostname { get; set; }

        public HostNameNoExistException() : 
            base("hostname doesn't exist.")            
        {
        }

        public HostNameNoExistException(string hostname) 
            : base($"Hostname {hostname} is not an userName.")
        {
            Hostname = hostname;
        }

        public HostNameNoExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected HostNameNoExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}