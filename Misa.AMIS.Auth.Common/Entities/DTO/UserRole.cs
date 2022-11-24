using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Auth.Common.Entities.DTO
{
    public class UserRole
    {
        /// <summary>
        /// Tài khoản người dùng
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Tên vai trò
        /// </summary>
        public IEnumerable<string> RoleName { get; set; }
    }
}
