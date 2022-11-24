using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Common.Entities.DTO
{
    public class FileResponse
    {
        /// <summary>
        /// Thành công hay thất bại
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Dữ liệu đi kèm khi thành công hoặc thất bại
        /// </summary>
        public FileAccess fileAccess { get; set; }
    }
}
