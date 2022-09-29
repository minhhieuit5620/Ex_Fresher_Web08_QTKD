using Ex_HMH_Common.Entities;
using Ex_QTKD_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_HMH_BL
{
    public interface IEmployeeBL
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
        /// Lấy một  nhân viên 
        /// </summary>      
        /// <param name="employeeID"></param>
        /// <returns>dữ liệu nhân viên tương ứng với mã nhân viên đã nhập</returns>   
        /// Created by: HMHieu(98/09/2022)
        public Employee EmployeeByID(Guid employeeID);

        /// <summary>
        ///  thêm mới một nhân viên 
        /// </summary>
        /// <param name="employee"> Đối tượng nhân viên cần thêm mới</param> 
        /// <returns>Return về 1 nếu thêm mới thành công, 0 nếu thất bại</returns>
        /// Created by: HMHieu(28/09/2022)
        public ServiceResponse InsertEmployee(Employee employee);

        /// <summary>
        /// sửa một nhân viên 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="employeeID"></param>
        /// <param name="employee"></param>
        /// <returns>Return về Guid vừa sửa, Guid rỗng nếu thất bại </returns>  
        public ServiceResponse UpdateEmployee(Guid employeeID,Employee employee);

        /// <summary>
        /// xóa một nhân viên 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="employeeID"></param>
        /// <returns>Id của nhân viên vừa xóa </returns>
        public Guid DeleteEmployee(Guid employeeID);
    }
}
