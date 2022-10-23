using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// lấy toàn bộ giá trị bản ghi của một bảng
        /// </summary>
        /// <returns>tất cả các giá trị của bảng được nhập</returns>   
        /// CreatedBy: HMHieu (29-09-2022)
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// Lấy danh sách bản ghi theo filter
        /// </summary>
        /// <param name="search">chuỗi cần tìm kiếm</param>
        /// <param name="sort">sắp xếp</param>
        /// <param name="pageIndex">số trang </param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <returns>Danh sách bản ghi thỏa mãn điều kiện filter</returns>
        /// Created by: HMHieu(28/09/2022)
        public PagingData<T> Filter(string? search, string? sort, int pageIndex, int pageSize);

        /// <summary>
        ///lấy một bản ghi theo mã
        /// </summary>      
        /// <param name="ID"></param>
        /// <returns>dữ liệu bản ghi tương ứng với mã đã nhập</returns>
        /// Created by: HMHieu(29/09/2022)
        public T RecordByID(Guid ID);

        /// <summary>
        ///  thêm mới một bản ghi
        /// </summary>
        /// <param name="record"> Đối tượng cần thêm mới</param> 
        /// <returns>Return về Guid mới, Guid rỗng nếu thất bại</returns>
        /// Created by: HMHieu(28/09/2022)
        public string Insert(T record);

        /// <summary>
        /// sửa một bản ghi 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="ID"></param>
        /// <param name="record"></param>
        /// <returns>Return về Guid vừa sửa, Guid rỗng nếu thất bại </returns>      
        public string Update(Guid ID, T record);

        /// <summary>
        /// xóa một bản ghi 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="ID"></param>
        /// <returns>Id của bản ghi vừa xóa </returns>
        public Guid Delete(Guid ID);

        /// <summary>
        /// Kiểm tra mã trùng
        /// CreatedBy: HMHieu(9/10/2022)
        /// </summary>
        /// <param name="record">bản ghi nhân viên</param>
        /// <returns>IDs nếu bị trùng</returns>
        public Guid checkDuplicateEmployeeCode(T record);
    }
}
