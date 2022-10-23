
using Misa.AMIS.BL.BaseBL;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.DL.DeparmentDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.BL.DepartmentBL
{
    public class DepartmentBL:BaseBL<Department>,IDepartmentBL
    {
        #region Feild

        private IDeparmentDL _departmentDL;

        #endregion

        #region Contractor

        public DepartmentBL(IDeparmentDL departmentDL):base(departmentDL)
        {
            _departmentDL = departmentDL;
        }

        #endregion
        /// <summary>
        ///lấy danh sách phòng ban 
        /// </summary>              
        /// <returns>dữ liệu danh sách phòng ban</returns>
        /// Created by: HMHieu(29/09/2022)
        //public IEnumerable<Department> AllDepartment()
        //{
        //    return _departmentDL.AllDepartment();
        //}
    }
}
