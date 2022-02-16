using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckCity.Domain.Roles
{
    public class NbEachRole
    {
        public string RoleName { get; set; }
        public int Number { get; set; }

        public NbEachRole(string roleName, int number)
        {
            RoleName = roleName;
            Number = number;
        }
    }
}
