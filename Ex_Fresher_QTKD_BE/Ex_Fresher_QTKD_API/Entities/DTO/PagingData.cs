﻿namespace Ex_Fresher_QTKD_API.Entities.DTO
{

    /// <summary>
    /// Dữ liệu trả về khi lọc và phân trang
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng trong mảng trả về</typeparam>
    public class PagingData<T>
    {
      

            /// <summary>
            /// Mảng đối tượng thỏa mãn điều kiện lọc và phân trang
            /// </summary>
            public List<T> Data { get; set; } = new List<T>();

            /// <summary>
            /// Tổng số bản ghi thỏa mãn điều kiện
            /// </summary>
            public int TotalCount { get; set; }
<<<<<<< HEAD

            /// <summary>
            /// Tổng số bản ghi thỏa mãn điều kiện
            /// </summary>
            public int TotalPage { get; set; }


=======
        
>>>>>>> 96525cc7b746a980cf84aeddf40f1f7abdf071af
    }
}
