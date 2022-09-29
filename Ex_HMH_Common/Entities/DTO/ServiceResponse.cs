using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_HMH_Common.Entities 
{
    /// <summary>
    /// Dữ liệu trả về từ BL
    /// </summary>
    public class ServiceResponse
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
