using Misa.AMIS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Common.Entities
{
    public class User
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
