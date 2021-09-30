using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain.Models
{
    public class UserWithToken
    {
        public User user { get; set; }
        public String token { get; set; }
    }
}
