using Ex_QTKD_API.Attributes;

namespace Ex_QTKD_API.Entities
{
    public class Department
    {
        /// <summary>
        /// các thành phần của bảng phòng ban
        /// </summary>
        /// CreatedBy: Hứa minh hiếu(22-09-22)

        public class Deparment
        {
            /// <summary>
            /// ID phòng ban
            /// </summary>
            [Primarykey]
            public Guid DepartmentID { get; set; }

            /// <summary>
            /// Mã phòng ban
            /// </summary>
          
            [IsNotNullOrEmpty("Mã phòng ban không được để trống")]
            public string DeparmentCode { get; set; }

            /// <summary>
            /// Tên phòng ban
            /// </summary>
            [IsNotNullOrEmpty("Tên phòng ban không được để trống")]
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
}
