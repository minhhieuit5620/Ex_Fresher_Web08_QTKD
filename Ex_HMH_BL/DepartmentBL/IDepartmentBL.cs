
using Misa.AMIS.BL.BaseBL;
using Misa.AMIS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.BL.DepartmentBL
{
    public interface IDepartmentBL:IBaseBL<Department>
    {
        /// <summary>
        ///lấy danh sách phòng ban 
        /// </summary>              
        /// <returns>dữ liệu danh sách phòng ban</returns>
        /// Created by: HMHieu(29/09/2022)
      //  public IEnumerable<Department> AllDepartment();
    }
}
