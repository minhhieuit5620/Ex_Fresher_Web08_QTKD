using Ex_HMH_Common.Attributes;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Enums;
using Ex_HMH_Common;
using Ex_HMH_DL;
using Ex_QTKD_API.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ex_HMH_BL.BaseBL;

namespace Ex_HMH_BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Feild

        private IEmployeeDL _employeeDL;

        #endregion

        #region Contractor

        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// lấy giá trị mã nhân viên mới
        /// </summary>
        /// <returns>giá trị mã nhân viên mới</returns>
        /// CreatedBy Hứa Minh Hiếu(30-09-2022)
        public string NewEmployeeCode()
        {
            var maxCode = _employeeDL.NewEmployeeCode();

            string subMaxCode = maxCode.Substring(2);

            int resultPart = 0;

            bool result = int.TryParse(subMaxCode, out resultPart);

            if (result)
            {
                var newCode = "NV" + (resultPart + 1);

                return newCode;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Đổi trạng thái làm việc của nhân viên
        /// </summary>
        /// <param name="statusEmployee"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>ID của nhân viên thay đổi</returns>
        public Guid ChangeStatus(Status statusEmployee, Guid EmployeeID)
        {
            return _employeeDL.ChangeStatus(statusEmployee, EmployeeID);
        }


        #endregion


    }
}
