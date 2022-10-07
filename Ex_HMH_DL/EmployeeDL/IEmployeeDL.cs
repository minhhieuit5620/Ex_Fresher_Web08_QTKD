using Dapper;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Enums;
using Ex_QTKD_API.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ex_HMH_DL
{
    public interface IEmployeeDL: IBaseDL<Employee>
    {

        /// <summary>
        /// Lấy danh sách nhân viên theo filter
        /// </summary>
        /// <param name="search">chuỗi cần tìm kiếm</param>
        /// <param name="sort">sắp xếp</param>
        /// <param name="pageIndex">số trang </param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <returns>Danh sách nhân viên thỏa mãn điều kiện filter</returns>
        /// Created by: HMHieu(28/09/2022)
        public PagingData<Employee> FilterEmployee(string? search, string? sort, int pageIndex, int pageSize);

        /// <summary>
        /// Tạo mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// CreatedBy: Hứa Minh Hiếu
        public string NewEmployeeCode();

        /// <summary>
        /// Thay đổi trạng thái người dùng
        /// </summary>
        /// <returns>Guid nhân viên</returns>
        /// CreatedBy: Hứa Minh Hiếu
        public Guid ChangeStatus(Status statusEmployee, Guid EmployeeID);

        /// <summary>
        /// Xóa nhiều nhân viên cùng lúc
        /// </summary>
        /// <param name="listEmployeeID"></param>
        /// <returns>Số bản ghi đã xóa</returns>
        public int DeleteMultiple(List<Guid> listEmployeeID);
      
    }

}
