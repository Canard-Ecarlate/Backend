using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Application.Exceptions
{
    public class UsernameAlreadyExistException : Exception
    {
        private String userName { get; set; }

        public UsernameAlreadyExistException(String userName)
         : base(String.Format("Username {0} already exist in our database.", userName))
        {
            this.userName = userName;
        }
    }
}
