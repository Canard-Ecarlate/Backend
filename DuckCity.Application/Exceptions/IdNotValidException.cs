using System.Runtime.Serialization;

namespace DuckCity.Application.Exceptions
{
    [Serializable]
    public class IdNotValidException : Exception
    {
        private string? Id { get; set; }

        public IdNotValidException() : 
            base("ID is not valid")            
        {
        }

        public IdNotValidException(string? id) 
            : base($"ID : {id} is not valid")
        {
            Id = id;
        }

        public IdNotValidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected IdNotValidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}