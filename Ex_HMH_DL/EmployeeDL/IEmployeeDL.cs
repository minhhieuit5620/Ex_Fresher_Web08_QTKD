using Dapper;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Enums;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Misa.AMIS.DL
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
        /// Kiểm tra mã trùng
        /// CreatedBy: HMHieu(9/10/2022)
        /// </summary>
        /// <param name="record">bản ghi nhân viên</param>
        /// <returns>Mã code nếu bị trùng</returns>
        public Guid checkDuplicateEmployeeCode(Employee employee);

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

        /// <summary>
        /// kiểm tra dữ liệu đầu vào trong danh sách import đã có trong DB hay chưa
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>true nếu đã tồn tại/ false nếu chưa tồn tại</returns>
        public bool checkCodeImport(string employeeCode);

        /// <summary>
        /// nhập khẩu dữ liệu
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public int ImportData(List<Employee> employees);

        /// <summary>
        /// Nhập khẩu dữ liệu sử dụng Proc(test)
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public Task<int> ImportUsingProc(List<Employee> employees);
    }

}
