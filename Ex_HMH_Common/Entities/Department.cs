using Ex_HMH_Common.Attributes;

namespace Ex_HMH_Common.Entities
{
    /// <summary>
    /// các thành phần của bảng phòng ban
    /// </summary>
    /// CreatedBy: Hứa minh hiếu(22-09-22)
    public class Department:BaseEntities
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
            public string DepartmentCode { get; set; }

            /// <summary>
            /// Tên phòng ban
            /// </summary>
            [IsNotNullOrEmpty("Tên phòng ban không được để trống")]
            public string DepartmentName { get; set; }

      

    }
}
