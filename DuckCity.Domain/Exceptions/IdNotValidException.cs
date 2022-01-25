using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions
{
    [Serializable]
    public class IdNotValidException : Exception
    {
        private string? Id { get; set; }

        public IdNotValidException() : 
            base("ID is not an ObjectId")            
        {
        }

        public IdNotValidException(string? id) 
            : base($"ID : {id} is not an ObjectId")
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