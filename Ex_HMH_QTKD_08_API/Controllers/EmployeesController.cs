using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySqlConnector;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Attributes;
using Misa.AMIS.Common.Enums;
using Misa.AMIS.Common;

using System;

using NLog;

using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using System.Drawing;
using Microsoft.AspNetCore.Html;
using System.Reflection;
using OfficeOpenXml.Style;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Net;
using Misa.AMIS.BL;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Misa.AMIS.Common.Entities.DTO;
using Misa.AMIS.DL;
using Misa.AMIS.Common;
using System.Net.Mime;
using Misa.AMIS.Common.Handle;
//using Syncfusion.XlsIO;

namespace Misa.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BasesController<Employee>
    {

        #region Feild

        private readonly ILogger<Employee> _logger;

        private IEmployeeBL _employeeBL;

        #endregion

        #region Contructor               

        public EmployeesController(ILogger<Employee> logger, IEmployeeBL employeeBL) : base(logger, employeeBL)
        {
            _employeeBL = employeeBL;
            _logger = logger;
        }

        #endregion

        #region Method


        /// <summary>
        /// API lấy Filter nhân viên 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="departmentID"></param>
        /// <param name="sort"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns>
        /// + Danh sách nhân viên thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số nhân viên thỏa mã các điều kiện lọc
        /// </returns>
        /// Created by: HMHieu(18/09/2022)
        /// 
        [HttpGet("filter")]
        [Authorize(Roles = "Admin,User")]      
        public IActionResult FilterEmployee(
            [FromQuery] string? search,// dữ liệu cần tìm kiếm
            [FromQuery] string? sort,//sắp xếp
            [FromQuery] int pageIndex,// trang hiện tại
            [FromQuery] int pageSize)//số bản ghi trong 1 trang

        {
            try
            {
                var result = _employeeBL.FilterEmployee(search, sort, pageIndex, pageSize);

                //thành công
                if (result != null)
                {
                    int totalPage = 0;
                    //nếu số bản ghi trả về / số trang lẻ thì sẽ tạo thêm một trang
                    if ((result.TotalCount % pageSize) > 0)
                    {
                        totalPage = result.TotalCount / pageSize + 1;
                    }
                    else
                    {//nếu số bản ghi trả về vừa k lẻ
                        totalPage = result.TotalCount / pageSize;
                    }
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Employee>()
                    {
                        Data = result.Data.ToList(),//dữ liệu các bản ghi trả về 

                        TotalCount = result.TotalCount,//tổng số bản ghi trả về
                       
                        TotalPage = totalPage// tổng số trang sau khi đã phân trang trả về

                    });
                }//thất bại
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput));
                }
            }
            catch (Exception exception)
            {
                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(exception);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError,HandleError.HandleErrorResult(ErrorCode.Exception));
            }

        }



        /// <summary>
        /// API tạo mã nhân viên mới
        /// </summary>
        /// <returns>mã nhân viên mới nếu thành công, null nếu thất bại</returns>
        /// CreatedBy: Hứa minh hiếu (24-09-2022)
        [HttpGet("new-code-employee")]
        public IActionResult newCodeEmployee()
        {
            try
            {
                var maxCode = _employeeBL.NewEmployeeCode();

                if (maxCode.Success)//thành công
                {                    
                    return StatusCode(StatusCodes.Status200OK, maxCode);
                }
                //thất bại
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput));
                }
            }
            catch (Exception ex)
            {
                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));

            }
        }

        /// <summary>
        /// API thay đổi trạng thái làm việc của nhân viên
        /// </summary>
        /// <param name="statusEmployee"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>ID của nhân viên thay đổi</returns>
        [HttpPut("change-status")]
        public IActionResult ChangeStatus(Status statusEmployee, Guid EmployeeID)
        {
            try
            {
                var employeeChange = _employeeBL.ChangeStatus(statusEmployee, EmployeeID);

                if (employeeChange.Success)//thành công
                {
                    return StatusCode(StatusCodes.Status200OK, employeeChange);
                }
                else//thất bại
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput));
                }
            }
            catch (Exception ex)
            {
                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }
        }

        /// <summary>
        /// API lấy xuất khẩu dữ liệu ra excel
        /// </summary>
        /// <returns>file excel</returns>
        /// CreatedBy Hứa Minh Hiếu (03-10-2022)
        [HttpGet("export-excel")]
        [Authorize(Roles = "Admin")]
        public IActionResult Export([FromQuery] string? search)
        {
            try
            {       //epplus

                int limit = -1;// để lấy toàn bộ các bản ghi( được quy định trong proc)

                int offset = 1;// lấy từ bản ghi đầu tiên              

                string sort = "";//sắp xếp

               var listEmployee = _employeeBL.FilterEmployee(search, sort, offset, limit);//sử dụng hàm filter để lấy data export

                List<Employee> listExport = listEmployee.Data;//chuyển data sau khi filter sang list 

                //listEmployee = _employeeBL.GetAllRecords();

                //kiểm tra dữ liệu đầu vào 
                if (listExport == null)//nếu rỗng thì break;
                {
                    //trả về status code và gửi mã lỗi được quy định trước đó về hàm handle
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput)) ;
                }
                else//nếu có dữ liệu
                {
                    var result =  _employeeBL.ExportExcel(listExport);

                    if (result.Success)
                    {
                        var contentType = Resource.ContenTypeExport;
                        var fileName = Resource.FileNameExport + ".xlsx";
                        var stream = (byte[])result.Data;
                        return File(stream, contentType, fileName);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput));
                    }                  
                }
            }
            catch (Exception ex)
            {
                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }
        }

        /// <summary>
        /// Xóa nhiều nhân viên cùng lúc
        /// </summary>
        /// <param name="listEmployeeID"></param>
        /// <returns>Số bản ghi đã xóa</returns>
        [HttpPost("delete-multiple")]
        public IActionResult Delete_Multiple(List<Guid> listEmployeeID)
        {
            try
            {
                var numberEmployeeDelete = _employeeBL.DeleteMultiple(listEmployeeID);

                if (numberEmployeeDelete.Success)//thành công
                {
                    return StatusCode(StatusCodes.Status200OK, numberEmployeeDelete);
                }
                else//thất bại
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput));
                }
            }
            catch (Exception ex)
            {
                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }

        }

        /// <summary>
        /// Kiểm tra dữ liệu nhập khẩu ( 3 bước)
        /// - nhận file dữ liệu từ FE gửi
        /// </summary>
        /// <param name="formFile">file dữ liệu người dùng muốn gửi</param>
        /// <returns>dữ liệu sau khi validate </returns>
        [HttpPost("check-import")]
        public async Task<IActionResult> CheckImport([FromForm(Name = "file")] IFormFile formFile)
        {
            try
            {
                if (formFile == null || formFile.Length <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.NotExitsFile));
                }                      
                var result = await _employeeBL.CheckDataThreeStep(formFile);

                if (result.Success)
                {
                    //Trả về kết quả thành công
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }

            }
            catch (Exception ex)
            {

                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }
           
        }

        /// <summary>
        /// Nhập khẩu dữ liệu người dùng( 3 bước)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("import-steps")]
        public IActionResult ImportThreeSteps(List<string> list)
        {            
            try
            {
                if (list == null || list.Count == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput));

                }
                else
                {
                    var result =  _employeeBL.ImportThreeSteps(list);

                    if (result.Success)
                    {

                        return StatusCode(StatusCodes.Status200OK, result);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,  result);
                    }
                }
            }
            catch (Exception ex)
            {

                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }
            
        }
        /// <summary>
        /// Xuất khẩu dữ liệu các bản ghi đã nhập khẩu thành công
        /// </summary>
        /// <returns>file excel nếu thành công/ lỗi nếu thất bại</returns>
        [HttpGet("export-import")]
        public IActionResult ExportFileImportSuccess()
        {
            try
            {                
                var result = _employeeBL.ExportFileImportSuccess();
                
                if (result.Success)
                {
                    var contentType = Resource.ContenTypeExport;
                    var fileName = Resource.FileNameExport+".xlsx";                 
                    var stream = (byte[])result.Data;
                    return File(stream, contentType, fileName);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,result);
                }
            }
            catch (Exception ex)
            {

                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));

            }
        }


        /// <summary>
        /// nhập khẩu dữ liệu (1 bước)
        /// </summary>
        /// <param name="formFile">file excel </param>
        /// <returns>thành công hoặc thất bại</returns>
        [HttpPost("import-default")]
        public IActionResult ImportDefault([FromForm(Name = "file")] IFormFile formFile)
        {
            try
            {                
                //kiểm tra ó file hay k?
                if (formFile == null || formFile.Length <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.NotExitsFile));
                }                

                var result =  _employeeBL.ImportData(formFile);

                if (result.Success)
                {
                    // Trả về kết quả thành công
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }

            }
            catch (Exception ex)
            {

                //Lưu lỗi vào nLog 
                HandleError.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }

        }

        #endregion
    }
}
