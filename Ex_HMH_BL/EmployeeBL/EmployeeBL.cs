

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Misa.AMIS.DL;
using Misa.AMIS.BL.BaseBL;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Enums;
using Misa.AMIS.Common;
using Misa.AMIS.DL.BaseDL;
using System.Resources;
using Misa.AMIS.Common;
using Misa.AMIS.Common.Attributes;
using System.Text.RegularExpressions;

namespace Misa.AMIS.BL
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
            return _employeeDL.FilterEmployee(search, sort, pageIndex, pageSize);
        }
    
        /// <summary>
        /// lấy giá trị mã nhân viên mới
        /// </summary>
        /// <returns>giá trị mã nhân viên mới</returns>
        /// CreatedBy Hứa Minh Hiếu(30-09-2022)
        public ServiceResponse NewEmployeeCode()
        {
            var maxCode = _employeeDL.NewEmployeeCode();

            //string subMaxCode = maxCode.Substring(2);

            //int resultPart = 0;

            //bool result = int.TryParse(subMaxCode, out resultPart);

            if (maxCode!=null||maxCode!="")
            {
                //var newCode = "NV" + (resultPart + 1);

                return new ServiceResponse
                {
                    Success = true,

                    Duplicate = false,

                    Data = maxCode
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,

                    Duplicate = false,

                    Data = new ErrorResult(

                        ErrorCode.InvalidEmployeeCode,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                };
            }
        }

        /// <summary>
        /// Đổi trạng thái làm việc của nhân viên
        /// </summary>
        /// <param name="statusEmployee"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>ID của nhân viên thay đổi</returns>
        public ServiceResponse ChangeStatus(Status statusEmployee, Guid EmployeeID)
        {
            var employee= _employeeDL.ChangeStatus(statusEmployee, EmployeeID);
            if (employee == Guid.Empty)
            {
                return new ServiceResponse
                {
                    Success = false,
                   
                    Data =  new ErrorResult(

                        ErrorCode.InvalidEmployeeCode,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Success = true,

                    Duplicate = false,

                    Data = employee
                };
            }
            //return _employeeDL.ChangeStatus(statusEmployee, EmployeeID);
        }

        /// <summary>
        /// Xóa nhiều nhân viên cùng lúc
        /// </summary>
        /// <param name="listEmployeeID"></param>
        /// <returns>Số bản ghi đã xóa</returns>
        public ServiceResponse DeleteMultiple(List<Guid> listEmployeeID)
        {
           var employee= _employeeDL.DeleteMultiple(listEmployeeID);
            if (employee > 0)
            {
                return new ServiceResponse
                {
                    Success = true,

                    Data = employee
                };
            }
            else
            {
                return new ServiceResponse
                {

                    Success = false,

                    Data = new ErrorResult(

                        ErrorCode.InvalidInput,

                        Resource.DevMsg_DeleteFailed,

                        Resource.UserMsg_DeleteFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                    
                };
            }
        }

        /// <summary>
        /// Validate riêng cho employee 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Nếu validate không thành công trả ra message, thành công trả về rỗng</returns>
        protected override List<string> ValidateObjectCustom(Employee entity)
        {
            // Kiểm tra đã nhập thông tin họ và tên hay chưa?
            if (string.IsNullOrEmpty(entity.EmployeeCode))
            {
                validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Mã nhân viên"));
            }
            if (string.IsNullOrEmpty(entity.EmployeeName))
            {
                validateFailed.Add(Resource.EmployeeNameNotNUll);
            }
            if (string.IsNullOrEmpty(entity.DepartmentID.ToString()))
            {
                validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Phòng ban"));
            }
            if (string.IsNullOrEmpty(entity.DepartmentName))
            {
                validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Phòng ban"));
            }

            // Kiểm tra xem mã nhân đã tồn tại hay chưa?
            Guid isDuplicate = _employeeDL.checkDuplicateEmployeeCode(entity);
            if (isDuplicate != Guid.Empty)
            {
                validateFailed.Add(Resource.UserMsg_DuplicateCode);

            }
            // định dạng mail
           // var isNotEmail = (IsNotEmailAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotEmailAttribute));
            string regexEmail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex email = new Regex(regexEmail);
            if ( entity.Email != null)
            {
                if (entity.Email != "")
                {
                    string emailValue = entity.Email.ToString();
                    //var a = email.IsMatch(emailValue);
                    if (emailValue != null && !email.IsMatch(emailValue))
                        validateFailed.Add(Resource.ErrorEmail);
                }
            }

            // validate ngày tháng
            //var isDate = (DateAttribute?)Attribute.GetCustomAttribute(property, typeof(DateAttribute));
            if (entity.DateOfBirth != null )
            {
                if ((DateTime)entity.DateOfBirth > DateTime.Now)
                {
                    validateFailed.Add( string.Format(Resource.ErrorMaxDate, "Ngày sinh"));
                }
            }
            if (entity.IdentityIssuedDate != null)
            {
                if ((DateTime)entity.IdentityIssuedDate > DateTime.Now)
                {
                    validateFailed.Add(string.Format(Resource.ErrorMaxDate, "Ngày cấp"));
                }
            }

            return validateFailed;
        }
        
        #endregion


    }
}
