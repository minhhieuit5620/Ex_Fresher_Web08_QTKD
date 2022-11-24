

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Misa.AMIS.DL;
using Misa.AMIS.BL.BaseBL;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Enums;
using Misa.AMIS.Common;
using Misa.AMIS.DL.BaseDL;
using System.Resources;
using Misa.AMIS.Common.Attributes;
using System.Text.RegularExpressions;
using Misa.AMIS.Common.Entities.DTO;
using System.Globalization;
using OfficeOpenXml;
using Microsoft.Extensions.Caching.Memory;
using System.Data.SqlTypes;
using static Dapper.SqlMapper;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Data;
using System.Runtime.CompilerServices;


namespace Misa.AMIS.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Feild

        private IEmployeeDL _employeeDL;

        private IMemoryCache _memoryCache;

        #endregion

        #region Contractor

        public EmployeeBL(IEmployeeDL employeeDL,IMemoryCache memoryCache) : base(employeeDL)
        {
            _employeeDL = employeeDL;            
            _memoryCache = memoryCache;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy danh sách nhân viên theo filter
        /// </summary>
        /// <param name="search">chuỗi cần tìm kiếm</param>
        /// <param name="sort">sắp xếp</param>
        /// <param name="pageIndex">số trang </param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <returns>Danh sách nhân viên thỏa mãn điều kiện filter</returns>
        /// Created by: HMHieu(28/09/2022)
        public PagingData<Employee> FilterEmployee(string? search, string? sort, int pageIndex, int pageSize)
        {
            return _employeeDL.FilterEmployee(search, sort, pageIndex, pageSize);
        }
    
        /// <summary>
        /// lấy giá trị mã nhân viên mới
        /// </summary>
        /// <returns>giá trị mã nhân viên mới</returns>
        /// CreatedBy Hứa Minh Hiếu(30-09-2022)
        public ServiceResponse NewEmployeeCode()
        {
            var maxCode = _employeeDL.NewEmployeeCode();

            //string subMaxCode = maxCode.Substring(2);

            //int resultPart = 0;

            //bool result = int.TryParse(subMaxCode, out resultPart);

            if (maxCode!=null||maxCode!="")
            {
                //var newCode = "NV" + (resultPart + 1);

                return new ServiceResponse
                {
                    Success = true,

                    Data = maxCode
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,                 

                    Data = new ErrorResult(

                        ErrorCode.InvalidEmployeeCode,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                };
            }
        }

        /// <summary>
        /// Đổi trạng thái làm việc của nhân viên
        /// </summary>
        /// <param name="statusEmployee"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>ID của nhân viên thay đổi</returns>
        public ServiceResponse ChangeStatus(Status statusEmployee, Guid EmployeeID)
        {
            var employee= _employeeDL.ChangeStatus(statusEmployee, EmployeeID);
            if (employee == Guid.Empty)
            {
                return new ServiceResponse
                {
                    Success = false,
                   
                    Data =  new ErrorResult(

                        ErrorCode.InvalidEmployeeCode,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Success = true,                    

                    Data = employee
                };
            }
            //return _employeeDL.ChangeStatus(statusEmployee, EmployeeID);
        }

        /// <summary>
        /// Xuất khẩu dữ liệu excel từ thông tin của bảng chính
        /// </summary>
        /// <param name="listExport">danh sách các bản ghi xuất khẩu</param>
        /// <returns></returns>
        public ServiceResponse ExportExcel(List<Employee> listExport)
        {
            if (listExport.Count>0)
            {
                var stream = new MemoryStream();

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Danh_sach_nhan_vien");

                    //căn trái dữ liệu
                    worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    //xét độ cao
                    worksheet.Row(2).Height = 18;

                    var startRow = 5;

                    var row = startRow;

                    worksheet.Cells["A1"].Value = "Danh sách nhân viên";// tên tiêu đề 

                    using (var r = worksheet.Cells["A1:J1"])// căn chỉnh dòng đầu nhằm mục đích đặt tiêu đề
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

                    using (var r = worksheet.Cells["A2:J2"])// dòng trống để cách tiêu đề vs table
                    {
                        r.Merge = true;
                    }
                    using (var r = worksheet.Cells["A3:J3"])// xây dựng background header table
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
                    //gán tên các cột
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
                    int start = 4;//gán giá trị ban đầu cho việc tạo border cho các hàng
                    int end = 4;//gán giá trị ban đầu cho việc tạo border cho các hàng

                    //lặp để truyền value vào bảng
                    foreach (var item in listExport)
                    {
                        string gender = "";
                        switch (item.Gender)// convert enum giới tính sang string
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
                        // truyền value vào từng hàng từng cột
                        worksheet.Cells[row, 1].Value = STT++;
                        worksheet.Cells[row, 2].Value = item.EmployeeCode;
                        worksheet.Cells[row, 3].Value = item.EmployeeName;
                        worksheet.Cells[row, 4].Value = gender;

                        //kiểm tra ngày sinh có trống hay không
                        if (item.DateOfBirth != null)//nếu khác rỗng thì thực hiện format
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
                        else//rỗng thì để trống
                        {
                            worksheet.Cells[row, 5].Value = "";
                        }
                        worksheet.Cells[row, 6].Value = item.PositionName;
                        worksheet.Cells[row, 7].Value = item.DepartmentName;
                        worksheet.Cells[row, 8].Value = item.BankAccount;
                        worksheet.Cells[row, 9].Value = item.BankName;
                        worksheet.Cells[row, 10].Value = item.BankBranch;

                        // tạo cell border
                        string modelRange = "A" + start++ + ":J" + end++;//gán tự động tăng theo số các bản ghi để lấy hàng vẽ border
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

                    //gán font chữ cho bảng
                    worksheet.Cells.Style.Font.Name = "Arial";

                    xlPackage.Save();

                    return new ServiceResponse
                    {
                        Success = true,
                        Data = xlPackage.GetAsByteArray()
                    };

                }
                //stream.Position = 0; 
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                    Data = (string.Format(Resource.ErrorValidNotNull, "Danh sách xuất khẩu"))
                };
            }                      
        }

        /// <summary>
        /// Xóa nhiều nhân viên cùng lúc
        /// </summary>
        /// <param name="listEmployeeID"></param>
        /// <returns>Số bản ghi đã xóa</returns>
        public ServiceResponse DeleteMultiple(List<Guid> listEmployeeID)
        {
           var employee= _employeeDL.DeleteMultiple(listEmployeeID);
            if (employee > 0)
            {
                return new ServiceResponse
                {
                    Success = true,

                    Data = employee
                };
            }
            else
            {
                return new ServiceResponse
                {

                    Success = false,

                    Data = new ErrorResult(

                        ErrorCode.InvalidInput,

                        Resource.DevMsg_DeleteFailed,

                        Resource.UserMsg_DeleteFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                    
                };
            }
        }

        /// <summary>
        /// Validate riêng cho employee 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Nếu validate không thành công trả ra message, thành công trả về rỗng</returns>
        protected override List<string> ValidateObjectCustom(Employee entity)
        {
            // Kiểm tra đã nhập thông tin mã nhân viên hay chưa?
            if (string.IsNullOrEmpty(entity.EmployeeCode))
            {
                validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Mã nhân viên"));
            }

            // Kiểm tra đã nhập thông tin họ và tên hay chưa?
            if (string.IsNullOrEmpty(entity.EmployeeName))
            {
                validateFailed.Add(Resource.EmployeeNameNotNUll);
            }

            // Kiểm tra đã nhập thông tin mã phòng ban hay chưa?
            if (string.IsNullOrEmpty(entity.DepartmentID.ToString()))
            {
                validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Phòng ban"));
            }

            // Kiểm tra đã nhập thông tin tên phòng ban hay chưa?
            if (string.IsNullOrEmpty(entity.DepartmentName))
            {
                validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Phòng ban"));
            }

            // Kiểm tra xem mã nhân đã tồn tại hay chưa?
            Guid isDuplicate = _employeeDL.checkDuplicateEmployeeCode(entity);
            if (isDuplicate != Guid.Empty)//Nếu tồn tại
            {
                validateFailed.Add(Resource.UserMsg_DuplicateCode);//add message để gửi cho người dùng

            }
            // định dạng mail
           // var isNotEmail = (IsNotEmailAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotEmailAttribute));
            string regexEmail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex email = new Regex(regexEmail);
            if ( entity.Email != null)//Nếu email khác null
            {
                if (entity.Email != "")
                {
                    string emailValue = entity.Email.ToString();
                    //var a = email.IsMatch(emailValue);
                    if (emailValue != null && !email.IsMatch(emailValue))
                        validateFailed.Add(Resource.ErrorEmail);//add message để gửi cho người dùng
                }
            }

            // validate ngày tháng
            //var isDate = (DateAttribute?)Attribute.GetCustomAttribute(property, typeof(DateAttribute));
            if (entity.DateOfBirth != null )//nếu ngày sinh khác null
            {
                if ((DateTime)entity.DateOfBirth > DateTime.Now)//Nếu ngày sinh lớn hơn ngày hiện tại
                {
                    validateFailed.Add( string.Format(Resource.ErrorMaxDate, "Ngày sinh", "hiện tại."));//Add message để gửi sang client
                }
            }
            if (entity.IdentityIssuedDate != null)//nếu ngày cấp khác null
            {
                if ((DateTime)entity.IdentityIssuedDate > DateTime.Now)//nếu ngày cấp lớn hơn ngày hiện tại
                {
                    validateFailed.Add(string.Format(Resource.ErrorMaxDate, "Ngày cấp","hiện tại."));//Add message 
                }
            }
            if (entity.IdentityIssuedDate != null && entity.DateOfBirth!=null)//nếu ngày cấp và ngày sinh khác null
            {
                if ((DateTime)entity.IdentityIssuedDate < (DateTime)entity.DateOfBirth)//nếu ngày cấp nhỏ hơn ngày sinh
                {
                    validateFailed.Add(string.Format(Resource.ErrorMaxDate, "Ngày cấp", "ngày sinh."));//Add message 
                }
            }

            return validateFailed;//return về danh sách các trường validate lỗi
        }

        /// <summary>
        /// Kiểm tra dữ liệu nhập khẩu theo 3 bước
        /// </summary>
        /// <param name="listEmployee">Danh sách nhân viên nhập khẩu</param>
        /// <returns>danh sách đối tượng được kiểm tra + các lỗi validate mà đối tượng đó đang có</returns>        
        public async Task<ServiceResponse> CheckDataThreeStep(IFormFile formFile)
        {

            var list = new List<EmployeesImport>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 4; row <= rowCount; row++)
                    {
                        Gender? gender = new Gender();
                        var convertGender = worksheet.Cells[row, 3].Value?.ToString() ?? string.Empty;
                        if(convertGender != string.Empty)
                        {
                            switch (convertGender)// convert enum giới tính sang string
                            {
                                case "Nam":
                                    gender = Gender.Male;
                                    break;
                                case "Nữ":
                                    gender = Gender.Female;
                                    break;
                                case "Khác":
                                    gender = Gender.Other;
                                    break;
                                default:
                                    gender = null;
                                    break;
                            }
                        }
                        else
                        {
                            gender = null;
                        }
                       
                        var dOB = worksheet.Cells[row, 4].Value?.ToString();
                        var identityDate= worksheet.Cells[row, 7].Value?.ToString();


                        var emp = new EmployeesImport()
                        {
                            EmployeeCode = worksheet.Cells[row, 1].Value?.ToString() ?? string.Empty ,
                            EmployeeName = worksheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                            Gender = gender,

                            DateOfBirth = dOB!=null &&
                                        DateTime.TryParseExact(dOB ?? string.Empty,
                                        "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth) ? dateOfBirth : null,
                            //DateTime.Parse(worksheet.Cells[row, 4].Value?.ToString(), cultureInfo),

                            DepartmentName = worksheet.Cells[row, 5].Value?.ToString() ?? string.Empty ,
                            IdentityNumber = worksheet.Cells[row, 6].Value?.ToString() ?? string.Empty ,

                            IdentityIssuedDate = identityDate != null&& 
                                DateTime.TryParseExact(identityDate ?? string.Empty,
                                "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime identityIssuedDate) ? identityIssuedDate : null,
                            //DateTime.Parse(worksheet.Cells[row, 7].Value?.ToString(), cultureInfo),


                            PositionName = worksheet.Cells[row, 8].Value?.ToString() ?? string.Empty,

                        };

                        if (emp == null)
                        {
                            continue;
                        }
                        else
                        {
                            list.Add(emp);
                        }
                        
                    }
                }
            }

            if (list == null)
            {
                return new ServiceResponse
                {
                    Data = string.Format(Resource.ErrorValidNotNull, Resource.File_Import),
                    Success = false
                };
            }
            else
            {
                //validate
                var listValidate =  ValidateObjectImport( list);

                return new ServiceResponse
                {
                    Data = listValidate,
                    Success = true
                };                
            }
        }

        /// <summary>
        /// Nhập khẩu dữ liệu vào DB
        /// </summary>
        /// <param name="list">Danh sách dữ liệu người dùng muốn thêm</param>
        /// <returns></returns>
        public ServiceResponse ImportThreeSteps(List<string> list)
        {
            //Tên cache
            string cacheKey = Resource.KeyCacheImport;          
          
            ////lấy dữu liệu từ cache
            _memoryCache.TryGetValue(cacheKey, out List<EmployeesImport> listCache);

            //danh sách lưu cache trống
            if (listCache == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Data = Resource.UserMsg_validateFailed,
                };
            }
            else
            {
                var listImport = new List<EmployeesImport>();

                //kiểm tra dữ liệu đầu vào trùng khớp với dữ liệu trên cache thì thêm vào list để import
                for (int i = 0; i < listCache.Count; i++)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j] == listCache[i].EmployeeCode)
                        {
                            listImport.Add(listCache[i]);
                            break;
                        }
                    }
                }
                //xóa cache
                _memoryCache.Remove(cacheKey);

                _memoryCache.Set(Resource.KeyCacheImport, listImport);

                //chuyển kiểu sang employee
                var listEmp = new List<Employee>();
                foreach (var employee in listImport)
                {
                    var emp = new Employee
                    {
                        EmployeeCode = employee.EmployeeCode,
                        EmployeeName = employee.EmployeeName,
                        DateOfBirth = employee.DateOfBirth,
                        Gender = employee.Gender,
                        DepartmentName = employee.DepartmentName,
                        IdentityNumber = employee.IdentityNumber,
                        IdentityIssuedDate = employee.IdentityIssuedDate,
                        PositionName = employee.PositionName
                    };
                    listEmp.Add(emp);

                }


                if (listEmp != null)
                {

                    //var result = await _employeeDL.ImportUsingProc(listEmp);
                    var result =  _employeeDL.ImportData(listEmp);

                    if (result == listEmp.Count)//thành công
                    {
                        return new ServiceResponse
                        {
                            Success = true,
                            Data = result
                        };
                    }
                    else
                    {
                        return new ServiceResponse
                        {
                            Success = false,
                            Data = result
                        };
                    }
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Data = Resource.UserMsg_validateFailed
                    };

                }
            }

        }

        /// <summary>
        /// Xuất khẩu file dữ liệu nhập khẩu thành công
        /// </summary>
        /// <returns></returns>
        public  ServiceResponse ExportFileImportSuccess()
        {
            //lấy dữu liệu từ cache
            string cacheKey = Resource.KeyCacheImport;
            _memoryCache.TryGetValue(cacheKey, out List<EmployeesImport> listExport);
          
            if (listExport.Count > 0)
            {
                var stream = new MemoryStream();

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Danh_sach_nhan_vien");

                    //căn trái dữ liệu
                    worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    //xét độ cao
                    worksheet.Row(2).Height = 18;

                    var startRow = 5;

                    var row = startRow;

                    worksheet.Cells["A1"].Value = "Danh sách nhân viên";// tên tiêu đề 

                    using (var r = worksheet.Cells["A1:I1"])// căn chỉnh dòng đầu nhằm mục đích đặt tiêu đề
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

                    using (var r = worksheet.Cells["A2:I2"])// dòng trống để cách tiêu đề vs table
                    {
                        r.Merge = true;
                    }
                    using (var r = worksheet.Cells["A3:I3"])// xây dựng background header table
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
                    //gán tên các cột
                    worksheet.Cells["A3"].Value = "STT";
                    worksheet.Cells["B3"].Value = "Mã nhân viên";
                    worksheet.Cells["C3"].Value = "Tên nhân viên";
                    worksheet.Cells["D3"].Value = "Giới tính";
                    worksheet.Cells["E3"].Value = "Ngày sinh";
                    worksheet.Cells["F3"].Value = "Phòng ban";
                    worksheet.Cells["G3"].Value = "Số CMND";
                    worksheet.Cells["H3"].Value = "Ngày cấp";
                    worksheet.Cells["I3"].Value = "Vị trí";


                    row = 4;
                    int STT = 1;
                    int start = 4;//gán giá trị ban đầu cho việc tạo border cho các hàng
                    int end = 4;//gán giá trị ban đầu cho việc tạo border cho các hàng

                    //lặp để truyền value vào bảng
                    foreach (var item in listExport)
                    {
                        string gender = "";
                        switch (item.Gender)// convert enum giới tính sang string
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
                        // truyền value vào từng hàng từng cột
                        worksheet.Cells[row, 1].Value = STT++;
                        worksheet.Cells[row, 2].Value = item.EmployeeCode;
                        worksheet.Cells[row, 3].Value = item.EmployeeName;
                        worksheet.Cells[row, 4].Value = gender;

                        //kiểm tra ngày sinh có trống hay không
                        if (item.DateOfBirth != null)//nếu khác rỗng thì thực hiện format
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
                        else//rỗng thì để trống
                        {
                            worksheet.Cells[row, 5].Value = "";
                        }
                        worksheet.Cells[row, 6].Value = item.DepartmentName;
                        worksheet.Cells[row, 7].Value = item.IdentityNumber;
                        if (item.IdentityIssuedDate != null)//nếu khác rỗng thì thực hiện format
                        {
                            //format date
                            var date = item.IdentityIssuedDate.Value.Day;
                            var month = item.IdentityIssuedDate.Value.Month;
                            var year = item.IdentityIssuedDate.Value.Year;
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
                            worksheet.Cells[row, 8].Value = formatDay + "/" + formatMonth + "/" + year.ToString();
                        }
                        else//rỗng thì để trống
                        {
                            worksheet.Cells[row, 8].Value = "";
                        }
                        //worksheet.Cells[row, 8].Value = item.IdentityIssuedDate;
                        worksheet.Cells[row, 9].Value = item.PositionName;

                        // tạo cell border
                        string modelRange = "A" + start++ + ":I" + end++;//gán tự động tăng theo số các bản ghi để lấy hàng vẽ border
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
                    //worksheet.Column(10).Width = 26;
                    worksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // worksheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    //gán font chữ cho bảng
                    worksheet.Cells.Style.Font.Name = "Arial";

                    xlPackage.Save();

                    return new ServiceResponse
                    {
                        Success = true,
                        Data = xlPackage.GetAsByteArray()
                        

                    };
                }            
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                    Data = Resource.UserMsg_validateFailed

                };
            }
           
        }      
            
        /// <summary>
        /// Nhập khẩu dữ liệu một bước
        /// </summary>
        /// <param name="listEmployee">Danh sách nhân viên nhập khẩu</param>
        /// <returns></returns>        
        public ServiceResponse ImportData(IFormFile formFile)
        {
            var list = new List<EmployeesImport>();

            using (var stream = new MemoryStream())
            {
                 formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 4; row <= rowCount; row++)
                    {
                        var gender = new Gender();
                        var convertGender = worksheet.Cells[row, 3].Value?.ToString().Trim();
                        switch (convertGender)// convert enum giới tính sang string
                        {
                            case "Nam":
                                gender = Gender.Male;
                                break;
                            case "Nữ":
                                gender = Gender.Female;
                                break;
                            case "Khác":
                                gender = Gender.Other;
                                break;

                        }
                        var cultureInfo = new CultureInfo("de-DE");
                        list.Add(new EmployeesImport()
                        {
                            EmployeeCode = worksheet.Cells[row, 1].Value?.ToString().Trim(),
                            EmployeeName = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                            Gender = gender,
                            
                            DateOfBirth = DateTime.Parse(worksheet.Cells[row, 4].Value?.ToString(), cultureInfo),
                        //DateTime.TryParseExact(worksheet.Cells[row, 4].Value?.ToString(),
                          //                  "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth) ? dateOfBirth : null,
                            DepartmentName = worksheet.Cells[row, 5].Value?.ToString().Trim(),
                            IdentityNumber = worksheet.Cells[row, 6].Value?.ToString().Trim(),
                            IdentityIssuedDate = DateTime.Parse(worksheet.Cells[row, 7].Value?.ToString(), cultureInfo),
                            //DateTime.TryParseExact(worksheet.Cells[row, 7].Value.ToString(),
                            //                    "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime identityIssuedDate) ? identityIssuedDate : null,

                            PositionName = worksheet.Cells[row, 8].Value?.ToString().Trim(),
                        });
                    }
                }
            }

            if (list == null)
            {
                return new ServiceResponse
                {
                    Data = string.Format(Resource.ErrorValidNotNull, "file nhập khẩu"),
                    Success = false
                };
            }
            else
            {
                //validate
                var listValidate =  ValidateObjectImportDefault(list);

                if (listValidate.Count > 0)
                {
                    return new ServiceResponse
                    {
                        Data = listValidate,
                        Success = false
                    };

                }
                else
                {                    
                    //chuyển sang Employee để thêm vào DB
                    var listEmp = new List<Employee>();
                    foreach (var employee in list)
                    {
                        var emp = new Employee
                        {
                            EmployeeCode = employee.EmployeeCode,
                            EmployeeName = employee.EmployeeName,
                            DateOfBirth = employee.DateOfBirth,
                            Gender = employee.Gender,
                            DepartmentName = employee.DepartmentName,
                            IdentityNumber = employee.IdentityNumber,
                            IdentityIssuedDate = employee.IdentityIssuedDate,
                            PositionName = employee.PositionName
                        };
                        listEmp.Add(emp);

                    }

                    var result =  _employeeDL.ImportData(listEmp);

                    if (result == listEmp.Count)//thành công
                    {
                        return new ServiceResponse
                        {
                            Success = true,
                            Data = result
                        };
                    }
                    else
                    {
                        return new ServiceResponse
                        {
                            Success = false,
                            Data = result
                        };
                    }
                }

            }
        }

        /// <summary>
        /// validate dữ liệu đầu vào file import cho nhập khẩu 1 bước
        /// </summary>
        /// <param name="listEmployee"></param>
        /// <returns></returns>
        protected List<string> ValidateObjectImportDefault(List<EmployeesImport> listEmployee)
        {
           
            for (int i = 0; i < listEmployee.Count; i++)
            {               

                // Kiểm tra đã nhập thông tin mã nhân viên hay chưa?
                if (string.IsNullOrEmpty(listEmployee[i].EmployeeCode))
                {
                    validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Mã nhân viên"));
                }

                // Kiểm tra đã nhập thông tin họ và tên hay chưa?
                if (string.IsNullOrEmpty(listEmployee[i].EmployeeName))
                {
                    validateFailed.Add(Resource.EmployeeNameNotNUll);
                }

                // Kiểm tra đã nhập thông tin tên phòng ban hay chưa?
                if (string.IsNullOrEmpty(listEmployee[i].DepartmentName))
                {
                    validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Phòng ban"));
                }

                // Kiểm tra đã nhập thông tin giới tính hay chưa?
                if (string.IsNullOrEmpty(listEmployee[i].Gender.ToString()))
                {
                    validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Giới tính"));
                }

                // Kiểm tra đã nhập thông tin ngày sinh hay chưa?
                if (string.IsNullOrEmpty(listEmployee[i].DateOfBirth.ToString()))
                {
                    validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Ngày sinh"));
                }

                // Kiểm tra đã nhập thông tin Số CMND hay chưa?
                if (string.IsNullOrEmpty(listEmployee[i].IdentityNumber))
                {
                    validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Số CMND"));
                }
                // Kiểm tra đã nhập thông tin ngày cấp hay chưa?
                if (string.IsNullOrEmpty(listEmployee[i].IdentityIssuedDate.ToString()))
                {
                    validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Ngày cấp"));
                }
                // Kiểm tra đã nhập thông tin tên phòng ban hay chưa?
                if (string.IsNullOrEmpty(listEmployee[i].PositionName))
                {
                    validateFailed.Add(string.Format(Resource.ErrorValidNotNull, "Vị trí"));
                }

                // validate ngày tháng
                //var isDate = (DateAttribute?)Attribute.GetCustomAttribute(property, typeof(DateAttribute));
                if (listEmployee[i].DateOfBirth != null)//nếu ngày sinh khác null
                {
                    if ((DateTime)listEmployee[i].DateOfBirth > DateTime.Now)//Nếu ngày sinh lớn hơn ngày hiện tại
                    {
                        validateFailed.Add(string.Format(Resource.ErrorMaxDate, "Ngày sinh", "hiện tại."));//Add message để gửi sang client
                    }
                }

                if (listEmployee[i].IdentityIssuedDate != null)//nếu ngày cấp khác null
                {
                    if ((DateTime)listEmployee[i].IdentityIssuedDate > DateTime.Now)//nếu ngày cấp lớn hơn ngày hiện tại
                    {
                        validateFailed.Add(string.Format(Resource.ErrorMaxDate, "Ngày cấp", "hiện tại."));//Add message 
                    }
                }

                if (listEmployee[i].IdentityIssuedDate != null && listEmployee[i].DateOfBirth != null)//nếu ngày cấp và ngày sinh khác null
                {
                    if ((DateTime)listEmployee[i].IdentityIssuedDate < (DateTime)listEmployee[i].DateOfBirth)//nếu ngày cấp nhỏ hơn ngày sinh
                    {
                        validateFailed.Add(string.Format(Resource.ErrorMaxDate, "Ngày cấp", "ngày sinh."));//Add message 
                    }
                }

                //validateFailed dữ liệu Mã nhân viên
                string regexCode = @"^NV[0-9]+$";
                Regex empCode = new Regex(regexCode);
                if (listEmployee[i].EmployeeCode != null)
                {
                    if (listEmployee[i].EmployeeCode != "")
                    {
                        string employeeCodeValue = listEmployee[i].EmployeeCode;
                        //var a = email.IsMatch(emailValue);
                        if (employeeCodeValue != null && !empCode.IsMatch(employeeCodeValue))
                        {
                            validateFailed.Add(string.Format(Resource.ErrorValidNotFormat, "Mã nhân viên"));//add message để gửi cho người dùng
                        }
                    }
                }

                //validate dữ liệu số CMND là số
                string regexNumber = @"^[0-9]*$";
                Regex Identity = new Regex(regexNumber);
                if (listEmployee[i].IdentityNumber != null)
                {
                    if (listEmployee[i].IdentityNumber != "")
                    {
                        string employeeCodeValue = listEmployee[i].IdentityNumber;
                        //var a = email.IsMatch(emailValue);
                        if (employeeCodeValue != null && !Identity.IsMatch(employeeCodeValue))
                        {
                            validateFailed.Add(string.Format(Resource.ErrorValidNotFormat, "Số CMND"));//add message để gửi cho người dùng
                            //validateImport.StringError.Add(string.Format(Resource.ErrorValidNotFormat, "Số CMND"));
                        }
                    }
                }
               
            }
            return validateFailed;//return về danh sách các trường validate lỗi
        }

        /// <summary>
        /// validate dữ liệu đầu vào file import 3 bước
        /// </summary>
        /// <param name="listEmployee"></param>
        /// <returns></returns>
        protected List<ImportServiceResponse> ValidateObjectImport(List<EmployeesImport>  listEmployee)
        {
            //khởi tạo danh sách trả về
            var listValidateImport = new List<ImportServiceResponse>();

            Dictionary<string, EmployeesImport> AuthorList = new Dictionary<string, EmployeesImport>();

            //khởi tạo danh sách để lưu dữ liệu vào cache
            var list = new List<EmployeesImport>();

            //lấy các thuộc tính của đối tượng
            var properties = typeof(EmployeesImport).GetProperties();
            

            for (int i = 0; i < listEmployee.Count; i++)
            {
                //danh sách các lỗi trả về
                var listCheck=new List<string>();
              
                foreach (var property in properties)
                {
                    // Lấy tất cả thuộc tính của đối tượng
                    var propertyValue = property.GetValue(listEmployee[i]);

                    // Lấy thuộc tính không được null hoặc rỗng
                    var isNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));

                    if (isNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                    {
                        listCheck.Add(isNotNullOrEmptyAttribute.ErrorMessage);
                    }
                }

                // validate ngày tháng
                //var isDate = (DateAttribute?)Attribute.GetCustomAttribute(property, typeof(DateAttribute));
                if (listEmployee[i].DateOfBirth != null)//nếu ngày sinh khác null
                {
                    if ((DateTime)listEmployee[i].DateOfBirth > DateTime.Now)//Nếu ngày sinh lớn hơn ngày hiện tại
                    {
                        listCheck.Add(string.Format(Resource.ErrorMaxDate, "Ngày sinh", "hiện tại."));//Add message để gửi sang client
                    }
                }

                if (listEmployee[i].IdentityIssuedDate != null)//nếu ngày cấp khác null
                {
                    if ((DateTime)listEmployee[i].IdentityIssuedDate > DateTime.Now)//nếu ngày cấp lớn hơn ngày hiện tại
                    {
                        listCheck.Add(string.Format(Resource.ErrorMaxDate, "Ngày cấp", "hiện tại."));//Add message 
                    }
                }

                if (listEmployee[i].IdentityIssuedDate != null && listEmployee[i].DateOfBirth != null)//nếu ngày cấp và ngày sinh khác null
                {
                    if ((DateTime)listEmployee[i].IdentityIssuedDate < (DateTime)listEmployee[i].DateOfBirth)//nếu ngày cấp nhỏ hơn ngày sinh
                    {
                        listCheck.Add(string.Format(Resource.ErrorMaxDate, "Ngày cấp", "ngày sinh."));//Add message 
                    }
                }

                // Kiểm tra xem mã nhân đã tồn tại hay chưa?
                if (listEmployee[i].EmployeeCode != null)
                {
                    var check = _employeeDL.checkCodeImport(listEmployee[i].EmployeeCode);
                    if (check)
                    {
                        listCheck.Add(Resource.UserMsg_DuplicateCode);//Add message 

                    }
                }          

                //validateFailed dữ liệu Mã nhân viên
                string regexCode = $"{Resource.RegexEmployeeCode}";                
                Regex empCode = new Regex(regexCode);
                if (listEmployee[i].EmployeeCode != null)
                {
                    if (listEmployee[i].EmployeeCode != "")
                    {
                        string employeeCodeValue = listEmployee[i].EmployeeCode;
                        //var a = email.IsMatch(emailValue);
                        if (employeeCodeValue != null && !empCode.IsMatch(employeeCodeValue))
                        {//add message để gửi cho người dùng
                            listCheck.Add(string.Format(Resource.ErrorValidNotFormat, "Mã nhân viên " + "'" + listEmployee[i].EmployeeCode + "'"));
                        }    
                    }
                }

                //validate dữ liệu số CMND là số
                string regexNumber = @$"{Resource.RegexNumber}";                   
                Regex Identity = new Regex(regexNumber);
                if (listEmployee[i].IdentityNumber != null)
                {
                    if (listEmployee[i].IdentityNumber != "")
                    {
                        string employeeCodeValue = listEmployee[i].IdentityNumber;
                        //var a = email.IsMatch(emailValue);
                        if (employeeCodeValue != null && !Identity.IsMatch(employeeCodeValue))
                        {//add message để gửi cho người dùng     
                            listCheck.Add(string.Format(Resource.ErrorValidNotFormat, "Số CMND "+ "'"+listEmployee[i].IdentityNumber+ "'"));                      
                        }    
                    }
                }
                //nếu mã nhân viên k trống
                if (!string.IsNullOrEmpty(listEmployee[i].EmployeeCode))
                {
                    //kiểm tra mã nhân viên đã tồn tại trên file nhập vào hay chưa
                    var checkCodeInFile = AuthorList.ContainsKey(listEmployee[i].EmployeeCode);

                    if (checkCodeInFile)//nếu đã tồn tại
                    {
                        //code = listEmployee[i].EmployeeCode;
                        listCheck.Add(string.Format(Resource.CodeExitsInFileImport, "'" + listEmployee[i].EmployeeCode + "'"));//add message để gửi cho người dùng
                    }
                    else//chưa tồn tại
                    {
                        AuthorList.Add(listEmployee[i].EmployeeCode, listEmployee[i]);
                    }
                }
                        

                //thêm vào danh sách trả về
                var employeeChecked = new ImportServiceResponse
                {
                    StringError = listCheck,
                    Data = listEmployee[i]
                };

                listValidateImport.Add(employeeChecked);

                //thêm các dữ liệu hợp lệ vào danh sách mới để lưu cache
                if ( listCheck.Count == 0)
                {
                    list.Add(listEmployee[i]);                   
                }

            }
            if(list.Count > 0)
            {
                //tên cache
                var cacheKey = Resource.KeyCacheImport;

                //lưu vào cache
                _memoryCache.Set(cacheKey, list);
                //_memoryCache.Set(cacheKey, AuthorList);
                //_memoryCache.TryGetValue(cacheKey, out Dictionary<string, object> listCache);

            }

            return listValidateImport;//return về danh sách các trường validate lỗi
        }

        #endregion


    }
}
