﻿using Ex_Fresher_QTKD_API.Enums;

namespace Ex_Fresher_QTKD_API.Entities
{
    /// <summary>
    /// các thành phần của bảng employee
    /// </summary>
<<<<<<< HEAD
    /// CreatedBy: Hứa Minh Hiếu(18-09-22)
=======
>>>>>>> 96525cc7b746a980cf84aeddf40f1f7abdf071af
    public class Employee
    {
        /// <summary>
        /// Id nhân viên
        /// </summary>
<<<<<<< HEAD
       // [PrimaryKey]
=======
>>>>>>> 96525cc7b746a980cf84aeddf40f1f7abdf071af
        public Guid EmployeeID { get; set; }

        /// <summary>
        /// Mã nhân viên 
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
            
        /// <summary>
        /// Số CMND
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CMND
        /// </summary>
        public DateTime? IdentityIssuedDate { get; set; }

        /// <summary>
        /// Nơi cấp CMND
        /// </summary>
        public string? IdentityIssuedPlace { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        public string? Mobile { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        public string? LandlinePhone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        public string? BankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string? BankBranch { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string? PositionNames { get; set; }


        /// <summary>
        /// ID phòng ban
        /// </summary>
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// Có phải là khách hàng không
        /// </summary>
        public bool? IsCustomer { get; set; }

        /// <summary>
        /// Có phải là nhà cung cấp không
        /// </summary>
        public bool? IsSupplier { get; set; }
       

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }

        
    }
}