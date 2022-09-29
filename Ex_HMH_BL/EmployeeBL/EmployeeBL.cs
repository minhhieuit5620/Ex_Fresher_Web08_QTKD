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

namespace Ex_HMH_BL
{
    public class EmployeeBL : IEmployeeBL
    {
        #region Feild

        private IEmployeeDL _employeeDL;

        #endregion

        #region Contractor

        public EmployeeBL (IEmployeeDL employeeDL)
        {
            _employeeDL = employeeDL;
        }

        #endregion

        /// <summary>
        /// Lấy danh sách nhân viên theo filter
        /// </summary>
        /// <param name="search">chuỗi cần tìm kiếm</param>
        /// <param name="sort">sắp xếp</param>
        /// <param name="pageIndex">số trang </param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <returns>Danh sách nhân viên thỏa mãn điều kiện filter</returns>
        /// Created by: HMHieu(28/09/2022)
        public PagingData<Employee> FilterEmployee(string? search, string? sort, int pageIndex, int pageSize)
        {
            return _employeeDL.FilterEmployee( search,  sort,  pageIndex,  pageSize);
        }

        public Employee EmployeeByID(Guid employeeID)
        {
            return _employeeDL.EmployeeByID(employeeID);
        }

        /// <summary>
        ///  thêm mới một nhân viên 
        /// </summary>
        /// <param name="employee"> Đối tượng nhân viên cần thêm mới</param> 
        /// <returns>Return về 1 nếu thêm mới thành công, 0 nếu thất bại</returns>
        /// Created by: HMHieu(28/09/2022)
        public ServiceResponse InsertEmployee(Employee employee)
        {
            var validateResults = ValidateRequestData(employee);
            if(validateResults != null && validateResults.Success)
            {
                var newEmployeeID=_employeeDL.InsertEmployee(employee);
                if (newEmployeeID != Guid.Empty)
                {
                    return new ServiceResponse
                    {
                        Data = newEmployeeID,
                        Success = true
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,

                        Data = new ErrorResult(

                        ErrorCode.InsertFailed,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                    };
                }
            }
            else
            {
                return new ServiceResponse
                {
                   Success=false,

                   Data=validateResults.Data
                };
            }          
        }

        /// <summary>
        /// sửa một nhân viên 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="employeeID"></param>
        /// <param name="employee"></param>
        /// <returns>Return về Guid vừa sửa, Guid rỗng nếu thất bại </returns>  
        public ServiceResponse UpdateEmployee(Guid employeeID,Employee employee)
        {
            var validateResults = ValidateRequestData(employee);

            if (validateResults != null && validateResults.Success)
            {
                var EmployeeID = _employeeDL.UpdateEmployee(employeeID,employee);

                if (EmployeeID != Guid.Empty)
                {
                    return new ServiceResponse
                    {
                        Data = EmployeeID,
                        Success = true
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,

                        Data = new ErrorResult(

                        ErrorCode.InsertFailed,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                    };
                }
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,

                    Data = validateResults.Data
                };
            }
        }

        /// <summary>
        /// xóa một nhân viên 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="employeeID"></param>
        /// <returns>Id của nhân viên vừa xóa </returns>
        public Guid DeleteEmployee(Guid employeeID)
        {
            return _employeeDL.DeleteEmployee(employeeID);
        }

        /// <summary>
        /// validate dữ liệu truyền lên từ API
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần Validate</param>
        /// <returns>Đối tượng ServiceResponse mô tả đối tượng validate thành công hay thất bại</returns>
        /// CreatedBy HMHieu(28-09-2022)
        private ServiceResponse ValidateRequestData(Employee employee)
        {
            //validate dữ liệu truyền vào
            var properties = typeof(Employee).GetProperties();

            var validateFailed = new List<string>();

            foreach (var property in properties)
            {
                string propertyName = property.Name;

                var propertyValue = property.GetValue(employee);

                var isNotNullOrEmptyAttr = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));

                //nếu có chứa attr và giá trị attr trống
                if (isNotNullOrEmptyAttr != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailed.Add(isNotNullOrEmptyAttr.ErrorMessage);
                }
            }

            if (validateFailed.Count > 0)
            {
                return new ServiceResponse
                {
                    Success = false,

                    Data = new ErrorResult(

                        ErrorCode.InvalidInput,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

                        String.Join(" ,", validateFailed)                     
                    )
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Success = true
                };
            }
        }

      
    }
}
