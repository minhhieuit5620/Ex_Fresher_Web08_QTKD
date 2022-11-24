using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Common.Entities.DTO
{
    public class ImportServiceResponse
    {
        /// <summary>
        /// Thành công hay thất bại
        /// </summary>
        public List<string> StringError { get; set; }

        /// <summary>
        /// Dữ liệu đi kèm khi thành công hoặc thất bại
        /// </summary>
        public object Data { get; set; }
    }
}
