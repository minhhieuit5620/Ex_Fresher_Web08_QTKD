
using Misa.AMIS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.BL.BaseBL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// lấy toàn bộ giá trị bản ghi của một bảng
        /// </summary>
        /// <param name="nameTable"></param>
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
        /// Thêm một bản ghi mới
        /// </summary>
        /// <param name="record">dữ liệu của bản ghi mới</param>
        /// <returns>ID của bản ghi</returns>
        /// CreatedBy Hứa Minh Hiếu
        public ServiceResponse Insert(T record);

        /// <summary>
        ///lấy một bản ghi theo mã
        /// </summary>      
        /// <param name="ID"></param>
        /// <returns>dữ liệu bản ghi tương ứng với mã đã nhập</returns>
        /// Created by: HMHieu(29/09/2022)
        public T RecordByID(Guid ID);

        /// <summary>
        /// sửa một bản ghi 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="ID"></param>
        /// <param name="record"></param>
        /// <returns>Return về Guid vừa sửa, Guid rỗng nếu thất bại </returns>      
        public ServiceResponse Update(Guid ID, T record);

        /// <summary>
        /// xóa một bản ghi 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="ID"></param>
        /// <returns>Id của bản ghi vừa xóa </returns>
        public ServiceResponse Delete(Guid ID);
    }
}
