using System;
using System.Runtime.Serialization;

namespace CanardEcarlate.Application.Exceptions
{
    [Serializable]
    public class PasswordConfirmationException : Exception
    {
        public PasswordConfirmationException() : 
            base("Password and password confirmation are not equals.")            
        {
        }

        public PasswordConfirmationException(string message)
            :base(message)
        {
        }

        public PasswordConfirmationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PasswordConfirmationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
