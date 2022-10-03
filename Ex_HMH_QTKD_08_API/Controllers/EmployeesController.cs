using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySqlConnector;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Attributes;
using Ex_HMH_Common.Enums;
using Ex_HMH_Common;
using Ex_HMH_BL;
using Ex_QTKD_API.Entities;
using System;
using Ex_HMH_DL;
using NLog;
using Ex_HMH_BL.BaseBL;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using System.Drawing;
using Microsoft.AspNetCore.Html;
using System.Reflection;
using OfficeOpenXml.Style;
using System.Globalization;
//using Syncfusion.XlsIO;

namespace Ex_HMH_QTKD_08_API.Controllers
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

                if (maxCode != "")
                {


                    return StatusCode(StatusCodes.Status200OK, maxCode);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        ErrorCode.InvalidEmployeeCode,
                        Resource.DevMsg_DuplicateCode,
                        Resource.UserMsg_DuplicateCode,
                        Resource.Moreinfo_InsertFailed
                        ));
                }



            }
            catch (Exception ex)
            {

                _logger.LogError("Error", ex);
                _logger.LogTrace("Trace", ex);

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

                if (employeeChange != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeChange);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        ErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_Exception,
                        Resource.Moreinfo_InsertFailed
                        ));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                _logger.LogTrace("Trace", ex);

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
                        // r.Style.Font.Color.SetColor(Color.B);
                        r.Style.Font.Size = 10;
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
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
                        worksheet.Cells[row, 5].Value = item.DateOfBirth.ToString();
                        worksheet.Cells[row, 6].Value = item.PositionName;
                        worksheet.Cells[row, 7].Value = item.DepartmentName;
                        worksheet.Cells[row, 8].Value = item.BankAccount?.ToString();
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

                    worksheet.Cells.AutoFitColumns();

                    worksheet.Cells.Style.Font.Name = "Arial";

                    xlPackage.Save();

                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Danh_sach_nhan_vien.xlsx");               
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
