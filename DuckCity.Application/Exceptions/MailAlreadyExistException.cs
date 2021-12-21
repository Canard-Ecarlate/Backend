using System.Runtime.Serialization;

namespace DuckCity.Application.Exceptions
{
    [Serializable]
    public class MailAlreadyExistException : Exception
    {
        private string? Mail { get; set; }

        public MailAlreadyExistException() : 
            base("mail already exists")            
        {
        }

        public MailAlreadyExistException(string? mail) 
            : base($"mail {mail} already exists in our database.")
        {
            Mail = mail;
        }

        public MailAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MailAlreadyExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}