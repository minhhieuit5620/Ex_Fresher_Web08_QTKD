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
        public IActionResult FilterEmployee(
            [FromQuery] string? search,
            [FromQuery] string? sort,
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize)

        {
            try
            {
                var result = _employeeBL.FilterEmployee(search, sort, pageIndex, pageSize);
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Employee>()
                    {
                        Data = result.Data.ToList(),

                        TotalCount = result.TotalCount,

                        TotalPage = result.TotalCount / pageSize + 1,

                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Error", exception);
                _logger.LogTrace("Trace", exception);
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }

        }



        /// <summary>
        /// API tạo mã nhân viên mới
        /// </summary>
        /// <returns>mã hân viên mới</returns>
        /// CreatedBy: Hứa minh hiếu (24-09-2022)
        [HttpGet("new-code-employee")]
        public IActionResult newCodeEmployee()
        {
            try
            {
                var maxCode = _employeeBL.NewEmployeeCode();

                if (maxCode.Success)
                {                    
                    return StatusCode(StatusCodes.Status200OK, maxCode);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        ErrorCode.InvalidInput,
                        Resource.DevMsg_InsertFailed,
                        Resource.UserMsg_InsertFailed,
                        Resource.Moreinfo_InsertFailed
                        ));
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Error", ex.Message);
                _logger.LogTrace("Trace", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);

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

                if (employeeChange.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeChange);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        ErrorCode.InvalidInput,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_Exception,
                        Resource.Moreinfo_InsertFailed
                        ));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.Message);
                _logger.LogTrace("Trace", ex.StackTrace);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// API lấy xuất khẩu dữ liệu ra excel
        /// </summary>
        /// <returns>file excel</returns>
        /// CreatedBy Hứa Minh Hiếu (03-10-2022)
        [HttpGet("export-excel")]
        public IActionResult Export()
        {
            try
            {       //epplus

                IEnumerable<Employee> listEmployee = new List<Employee>();

                listEmployee = _employeeBL.GetAllRecords();

                if (listEmployee == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                                          ErrorCode.InvalidInput,
                                          Resource.DevMsg_InsertFailed,
                                          Resource.UserMsg_InsertFailed,
                                          Resource.Moreinfo_InsertFailed,
                                          HttpContext.TraceIdentifier
                                     )); ;
                }
                else
                {
                    var stream = new MemoryStream();

                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        var worksheet = xlPackage.Workbook.Worksheets.Add("Danh_sach_nhan_vien");

                        worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        worksheet.Row(2).Height = 18;

                        var startRow = 5;

                        var row = startRow;

                        worksheet.Cells["A1"].Value = "Danh sách nhân viên";

                        using (var r = worksheet.Cells["A1:J1"])
                        {
                            r.Merge = true;

                            r.Style.Font.Color.SetColor(Color.Black);
                            r.Style.Font.Size = 16;

                            r.Style.Font.Bold = true;

                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            r.Style.Fill.BackgroundColor.SetColor(Color.White);

                            r.Style.Border.Top.Style = ExcelBorderStyle.Hair;
                            r.Style.Border.Left.Style = ExcelBorderStyle.Hair;
                            r.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                            r.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        }

                        using (var r = worksheet.Cells["A2:J2"])
                        {
                            r.Merge = true;
                        }
                        using (var r = worksheet.Cells["A3:J3"])
                        {
                            r.Style.Font.Size = 10;
                            r.Style.Font.Bold = true;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        worksheet.Cells["A3"].Value = "STT";
                        worksheet.Cells["B3"].Value = "Mã nhân viên";
                        worksheet.Cells["C3"].Value = "Tên nhân viên";
                        worksheet.Cells["D3"].Value = "Giới tính";
                        worksheet.Cells["E3"].Value = "Ngày sinh";
                        worksheet.Cells["F3"].Value = "Vị trí";
                        worksheet.Cells["G3"].Value = "Phòng ban";
                        worksheet.Cells["H3"].Value = "Số tài khoản";
                        worksheet.Cells["I3"].Value = "Tên ngân hàng";
                        worksheet.Cells["J3"].Value = "Chi nhánh ngân hàng";

                        row = 4;
                        int STT = 1;
                        int start = 4;
                        int end = 4;
                        foreach (var item in listEmployee)
                        {
                            string gender = "";
                            switch (item.Gender)
                            {
                                case Gender.Male:
                                    gender = "Nam";
                                    break;
                                case Gender.Female:
                                    gender = "Nữ";
                                    break;
                                case Gender.Other:
                                    gender = "Khác";
                                    break;
                            }

                            worksheet.Cells[row, 1].Value = STT++;
                            worksheet.Cells[row, 2].Value = item.EmployeeCode;
                            worksheet.Cells[row, 3].Value = item.EmployeeName;
                            worksheet.Cells[row, 4].Value = gender;

                            if (item.DateOfBirth != null)
                            {
                                //format date
                                var date = item.DateOfBirth.Value.Day;
                                var month = item.DateOfBirth.Value.Month;
                                var year = item.DateOfBirth.Value.Year;
                                string formatDay = "";
                                string formatMonth = "";
                                if (date < 10)
                                {
                                    string day = date.ToString();
                                    formatDay = "0" + day;
                                }
                                else
                                {
                                    formatDay = date.ToString();
                                }
                                if (month < 10)
                                {
                                    string mon = month.ToString();
                                    formatMonth = "0" + month;
                                }
                                else
                                {
                                    formatMonth = month.ToString();
                                }
                                worksheet.Cells[row, 5].Value = formatDay + "/" + formatMonth + "/" + year.ToString();
                            }
                            else
                            {
                                worksheet.Cells[row, 5].Value = "";
                            }
                            worksheet.Cells[row, 6].Value = item.PositionName;
                            worksheet.Cells[row, 7].Value = item.DepartmentName;
                            worksheet.Cells[row, 8].Value = item.BankAccount;
                            worksheet.Cells[row, 9].Value = item.BankName;
                            worksheet.Cells[row, 10].Value = item.BankBranch;

                            // tạo cell border
                            string modelRange = "A" + start++ + ":J" + end++;
                            var modelTable = worksheet.Cells[modelRange];

                            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            row++;
                        }
                        xlPackage.Workbook.Properties.Title = "DANH SÁCH NHÂN VIÊN";

                        xlPackage.Workbook.Properties.Author = " Hứa Minh Hiếu";

                        //format widtd and float 
                        worksheet.Cells.AutoFitColumns();
                        worksheet.Column(1).Width = 5;
                        worksheet.Column(2).Width = 16;
                        worksheet.Column(3).Width = 26;
                        worksheet.Column(4).Width = 12;
                        worksheet.Column(5).Width = 16;
                        worksheet.Column(6).Width = 20;
                        worksheet.Column(7).Width = 26;
                        worksheet.Column(8).Width = 16;
                        worksheet.Column(9).Width = 45;
                        worksheet.Column(10).Width = 26;
                        worksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                       // worksheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells.Style.Font.Name = "Arial";

                        xlPackage.Save();

                    }
                    stream.Position = 0;

                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Danh_sach_nhan_vien.xlsx");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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

                if (numberEmployeeDelete.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, numberEmployeeDelete);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                       ErrorCode.InvalidInput,
                       Resource.DevMsg_DeleteFailed,
                       Resource.UserMsg_DeleteFailed,
                       Resource.Moreinfo_InsertFailed
                       ));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.Message);
                _logger.LogTrace("Trace", ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }       
        #endregion
    }
}
