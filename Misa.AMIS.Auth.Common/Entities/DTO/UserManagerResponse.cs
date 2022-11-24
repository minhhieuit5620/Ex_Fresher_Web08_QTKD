using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Auth.Common.Entities.DTO
{
    public class UserManagerResponse
    {
        /// <summary>
        /// Thành công hay thất bại
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Dữ liệu đi kèm khi thành công hoặc thất bại
        /// </summary>
        public object Data { get; set; }
    }
}
