using System;

namespace CanardEcarlate.Application.Exceptions
{
    public class MailAlreadyExistException : Exception
    {
        private String mail { get; set; }

        public MailAlreadyExistException(String mail)
         : base(String.Format("mail {0} already exist in our database.",mail))
        {
            this.mail = mail;
        }
    }
}