
using Microsoft.AspNetCore.Http;
using Misa.AMIS.BL.BaseBL;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Entities.DTO;
using Misa.AMIS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.BL
{
    public interface IEmployeeBL: IBaseBL<Employee>
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
        public ServiceResponse NewEmployeeCode();

        /// <summary>
        /// Thay đổi trạng thái người dùng
        /// </summary>
        /// <returns>Guid nhân viên</returns>
        /// CreatedBy: Hứa Minh Hiếu
        public ServiceResponse ChangeStatus(Status statusEmployee, Guid EmployeeID);

        /// <summary>
        /// Xóa nhiều nhân viên cùng lúc
        /// </summary>
        /// <param name="listEmployeeID"></param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy: Hứa Minh Hiếu(05-10-2022)
        public ServiceResponse DeleteMultiple(List<Guid> listEmployeeID);

        /// <summary>
        /// Xuất khẩu dữ liệu excel từ bảng chính
        /// </summary>
        /// <param name="list">Danh sách bản ghi xuất khẩu</param>
        /// <returns></returns>
        public ServiceResponse ExportExcel(List<Employee> list);

        /// <summary>
        /// Nhập khẩu dữ liệu 1 bước
        /// </summary>
        /// <param name="listEmployee">File dữ liệu người dùng gửi</param>
        /// <returns></returns>
        /// CreatedBy;Hứa Minh Hiếu(11/11/2022)
        public  ServiceResponse ImportData(IFormFile formFile);


        /// <summary>
        /// Nhập khẩu dữ liệu theo 3 bước
        /// </summary>
        /// <param name="listEmployee">File dữ liệu người dùng gửi</param>
        /// <returns></returns>
        /// CreatedBy;Hứa Minh Hiếu(14/11/2022)
        public Task<ServiceResponse> CheckDataThreeStep(IFormFile formFile);

        /// <summary>
        /// Xuất khẩu file excel từ kết quả nhập khẩu thành công
        /// </summary>
        /// <returns></returns>
        public  ServiceResponse ExportFileImportSuccess();

        /// <summary>
        /// thực hiện thêm dữ liệu người dùng gửi vào DB
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public ServiceResponse ImportThreeSteps(List<string> list);

        
    }
}
