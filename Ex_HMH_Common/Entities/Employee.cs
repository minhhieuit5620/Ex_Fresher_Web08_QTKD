using Misa.AMIS.Common.Attributes;
using Misa.AMIS.Common;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Misa.AMIS.Common.Enums;

namespace Misa.AMIS.Common.Entities
{
    public class Employee:BaseEntities
    {
        /// <summary>
        /// các thành phần của bảng employee
        /// </summary>
        /// CreatedBy: Hứa Minh Hiếu(18-09-22)

        /// <summary>
        /// Id nhân viên
        /// </summary>
        [Primarykey]
        public Guid EmployeeID { get; set; }

        /// <summary>
        /// Mã nhân viên 
        /// </summary>
        [IsNotNullOrEmpty("Mã nhân viên không được để trống")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [IsNotNullOrEmpty("Tên nhân viên không được để trống")]
        public string EmployeeName { get; set; }
       
        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [Date("Ngày sinh không được lớn hơn hiện tại.")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số CMND
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CMND
        /// </summary>
        [Date("Ngày cấp không được lớn hơn hiện tại.")]
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
        [IsNotEmail("Không đúng định dạng email")]
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
        /// ID phòng ban
        /// </summary>
        [IsNotNullOrEmpty("Đơn vị không được để trống.")]
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [IsNotNullOrEmpty("Đơn vị không được để trống.")]
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
        /// Trạng thái làm việc
        /// </summary>
        public Status? Status { get; set; } =Enums.Status.Working;

        //public string Age
        //{
        //    get
        //    {
        //        return EmployeeName + EmployeeCode;
        //    }
        //    set
        //    {
        //        Age = value;
        //    }
        //}
    }
}
