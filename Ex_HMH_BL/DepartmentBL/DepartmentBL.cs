using Ex_HMH_BL.BaseBL;
using Ex_HMH_Common.Entities;
using Ex_HMH_DL;
using Ex_HMH_DL.DeparmentDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_HMH_BL.DepartmentBL
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
