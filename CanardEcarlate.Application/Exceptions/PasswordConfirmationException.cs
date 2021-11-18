using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Application.Exceptions
{
    public class PasswordConfirmationException : Exception
    {
        public PasswordConfirmationException() : 
            base("Password and password confirmation are not equals.")            
            {
            }
    }
}
