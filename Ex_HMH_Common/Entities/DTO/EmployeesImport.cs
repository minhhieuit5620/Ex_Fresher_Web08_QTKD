using Misa.AMIS.Common.Attributes;
using Misa.AMIS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Common.Entities.DTO
{
    public class EmployeesImport
    {
        /// <summary>
        /// các thành phần của bảng employee nhập khẩu
        /// </summary>
        /// CreatedBy: Hứa Minh Hiếu(18-09-22)
     
        /// <summary>
        /// Mã nhân viên 
        /// </summary>
        [IsNotNullOrEmpty("Mã nhân viên không được để trống")]       
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [IsNotNullOrEmpty("Tên nhân viên không được để trống")]
        public string? EmployeeName { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [IsNotNullOrEmpty("Giới tính không được để trống")]       
        public Gender? Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [Date("Ngày sinh không được lớn hơn hiện tại.")]
        [IsNotNullOrEmpty("Ngày sinh không được để trống")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số CMND
        /// </summary>
        [IsNotNullOrEmpty("Số CMND không được để trống")]
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CMND
        /// </summary>
        [Date("Ngày cấp không được lớn hơn hiện tại.")]
        [IsNotNullOrEmpty("Ngày cấp không được để trống")]
        public DateTime? IdentityIssuedDate { get; set; }       

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [IsNotNullOrEmpty("Đơn vị không được để trống.")]
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        [IsNotNullOrEmpty("Vị trí không được để trống.")]
        public string? PositionName { get; set; }

    }
}
