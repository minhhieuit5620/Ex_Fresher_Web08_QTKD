namespace Ex_Fresher_QTKD_API.Entities
{
    /// <summary>
    /// các thành phần của bảng phòng ban
    /// </summary>
<<<<<<< HEAD
    /// CreatedBy: Hứa minh hiếu(22-09-22)
=======
>>>>>>> 96525cc7b746a980cf84aeddf40f1f7abdf071af
    public class Deparment
    {
        /// <summary>
        /// ID phòng ban
        /// </summary>
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string DeparmentCode { get;set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }


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
