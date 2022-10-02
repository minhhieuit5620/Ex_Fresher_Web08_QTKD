using Ex_QTKD_API.Entities.DTO;
using Ex_QTKD_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySqlConnector;
using Ex_QTKD_API.Properties;
using System.Reflection;
using Ex_QTKD_API.Enums;
using Ex_QTKD_API.Attributes;

namespace Ex_QTKD_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
         //Khởi tạo kết nối tời DB MySQL
        string connectionString = "Server=localhost;Port=3306;Database=misa.qtkd.ex.hmhieu;Uid=root;Pwd=Hieu@5620;";

        #region get any table
        /// <summary>
        /// Hàm lấy toàn bộ giá trị của bất kỳ bảng nào
        /// </summary>
        /// <param name="nameTable"></param>
        /// <returns>tất cả các giá trị của bảng được nhập</returns>
        [HttpGet]
        [Route("getTable/{nameTable}")]
        public IActionResult AnyTable([FromRoute] string nameTable)
        {
            try
            {
                //Khởi tạo kết nối tời DB MySQL

                var mysqlConnection = new MySqlConnection(connectionString);

                string StoredProc = "Proc_employee_GetAllTable";

                //Chuẩn bị tham số đầu vào cho câu lệnh trên
                var parameters = new DynamicParameters();

                parameters.Add("Table_name", nameTable);

                //Thực hiện gọi vào DB

                var employee = mysqlConnection.Query(StoredProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về từ DB

                return StatusCode(StatusCodes.Status200OK, employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                //Resource.DevMsg_InsertFailed

                return StatusCode(StatusCodes.Status400BadRequest);

            }

        }

        #endregion

   
        #region Get FilterEmployee 
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
                var mySQLConnection = new MySqlConnection(connectionString);
                // Khởi tạo kết nối đến DB và gọi proc
                var procName = "Proc_employee_GetDataFilterPaging";

                //Chuẩn bị tham số đầu vào cho Proc

                var parameters = new DynamicParameters();

                parameters.Add("v_Offset", (pageIndex - 1) * pageSize);

                parameters.Add("v_Limit", pageSize);

                parameters.Add("v_Sort", sort);

                string vWhere = "";

                if (search != null)
                {
                    vWhere = $" EmployeeCode LIKE \'%{search}%\' OR FullName LIKE \'%{search}%\'  ";
                }
                parameters.Add("v_Where", vWhere);

                // Thực hiện gọi vào DB
                var results = mySQLConnection.QueryMultiple(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về 

                if (results != null)
                {
                    var employeeList = results.Read<Employee>();

                    var totalCount = results.Read<int>().Single();

                    return StatusCode(StatusCodes.Status200OK, new PagingData<Employee>()
                    {
                        Data = employeeList.ToList(),

                        TotalCount = totalCount,

                        TotalPage = totalCount / pageSize + 1,                  
   
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, exception);
            }

        }
        #endregion

        #region  Get EmployeeByID
        /// <summary>
        /// API lấy một  nhân viên 
        /// </summary>      
        /// <param name="employeeID"></param>
        /// <returns>dữ liệu nhân viên tương ứng với mã đã nhập</returns>
        /// <returns></returns>
        /// Created by: HMHieu(18/09/2022)
        [HttpGet("{employeeID}")]
        public IActionResult EmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                // tạo kết nối đến Db và gán tên store
                var mySQLConnection = new MySqlConnection(connectionString);

                var procname = "Proc_employee_GetEmployeeByID";

                // chuẩn bị các tham số truyền vào proc

                var parameters = new DynamicParameters();

                parameters.Add("v_EmployeeID", employeeID);

                //thưc hiện gọi vào DB 
                var employee = mySQLConnection.QueryFirstOrDefault(procname, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return StatusCode(StatusCodes.Status200OK, employee);

            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, exception);
            }

        }
        #endregion

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
                var mySQLConnection = new MySqlConnection(connectionString);

                var procName = "Proc_employee_GetMaxEmployeeCode";

                var maxCode = mySQLConnection.QueryFirstOrDefault<string>(procName, commandType: System.Data.CommandType.StoredProcedure);

                if (maxCode != null)
                {

                    string subMaxCode = maxCode.Substring(2);

                    var newCode = "NV" + (Int32.Parse(subMaxCode) + 1);

                    return StatusCode(StatusCodes.Status200OK, newCode);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest);

            }
        }

   
        #region  POST Employee
        /// <summary>
        /// API thêm mới một nhân viên 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>mã nhân viên</returns>
        /// <returns>Mã nhân viên tự động tăng</returns>
   
        /// Created by: HMHieu(18/09/2022)
        [HttpPost]
        public IActionResult Employee([FromBody] Employee employee)
        {
            try
            {
                //validate dữ liệu truyền vào
                var properties = typeof(Employee).GetProperties();

                var validateFailed = new List<string>();

                foreach (var property in properties)
                {
                    string propertyName = property.Name;

                    var propertyValue = property.GetValue(employee);

                    var isNotNullOrEmptyAttr = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));
                    
                    //nếu có chứa attr và giá trị attr trống
                    if (isNotNullOrEmptyAttr != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                    {
                        validateFailed.Add(isNotNullOrEmptyAttr.ErrorMessage);
                    }
                }
              
               if(validateFailed.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                            ErrorCode.InvalidInput,

                            Resource.DevMsg_InsertFailed,

                            Resource.DevMsg_validateFailed,

                            String.Join(" ,",validateFailed),

                            HttpContext.TraceIdentifier

                        ));
                }


                //Khởi tạo kết nối tới DB MySQL 
                string connectionString = "Server=localhost;Port=3306;Database=misa.qtkd.ex.hmhieu;Uid=root;Pwd=Hieu@5620;";
   
                var mysqlConnection = new MySqlConnection(connectionString);

                //Khai báo tên Proc

                string nameProc = "Proc_employee_InsertOne";           

                var guid = Guid.NewGuid();

                // chuẩn bị tham số đầu vào cho proc
                var parameters = new DynamicParameters();
   
                parameters.Add("v_EmployeeID", guid);

                parameters.Add("v_EmployeeCode", employee.EmployeeCode);

                parameters.Add("v_FullName", employee.FullName);

                parameters.Add("v_Gender", employee.Gender);

                parameters.Add("v_DateOfBirth", employee.DateOfBirth);

                parameters.Add("v_IdentityNumber", employee.IdentityNumber);

                parameters.Add("v_IdentityIssuedDate", employee.IdentityIssuedDate);

                parameters.Add("v_IdentityIssuedPlace", employee.IdentityIssuedPlace);

                parameters.Add("v_Address", employee.Address);

                parameters.Add("v_Mobile", employee.Mobile);

                parameters.Add("v_LandlinePhone", employee.LandlinePhone);

                parameters.Add("v_Email", employee.Email);

                parameters.Add("v_BankAccount", employee.BankAccount);

                parameters.Add("v_BankName", employee.BankName);

                parameters.Add("v_BankBranch", employee.BankBranch);

                parameters.Add("v_DepartmentID", employee.DepartmentID);

                parameters.Add("v_DepartmentName", employee.DepartmentName);

                parameters.Add("v_PositionName", employee.PositionName);

                parameters.Add("v_IsCustomer", employee.IsCustomer);

                parameters.Add("v_IsSupplier", employee.IsSupplier);

                parameters.Add("v_CreatedBy","Hứa Minh Hiếu");

                parameters.Add("v_CreatedDate", DateTime.Now);

                parameters.Add("v_ModifiedDate",DateTime.Now);

                var numberOfAffecttedRows = mysqlConnection.Execute(nameProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //xử lý dữ liệu trả về 
                //Thành công
                if (numberOfAffecttedRows > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, guid);
                }
                else  //Thát bại
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }                              

                //Try catch ex 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new ErrorResult(
                                             ErrorCode.Exception,
                                             Resource.DevMsg_Exception,
                                             Resource.UserMsg_Exception,
                                             Resource.Moreinfo_InsertFailed,
                                             HttpContext.TraceIdentifier
                                         )
                                  );

                //return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
        #endregion

        #region PUT Employee 
        /// <summary>
        /// API sửa một nhân viên 
        /// </summary>
        /// Created by: HMHieu(18/09/2022)
        /// <param name="employeeID"></param>
        /// <param name="employee"></param>
        /// <returns>nhân viên vừa sửa </returns>
        [HttpPut("{employeeID}")]
        public IActionResult Employee([FromRoute] Guid employeeID, [FromBody] Employee employee)
        {

            try
            {

                //tạo connection đến DB
                var mySQlConnection = new MySqlConnection(connectionString);
   
                var procName = "Proc_employee_UpdateByID";

                //chuẩn bị tham số đầu vào cho proc
                var parameters = new DynamicParameters();
   
                parameters.Add("v_EmployeeID", employee.EmployeeID);

                parameters.Add("v_EmployeeCode", employee.EmployeeCode);

                parameters.Add("v_FullName", employee.FullName);

                parameters.Add("v_Gender", employee.Gender);

                parameters.Add("v_DateOfBirth", employee.DateOfBirth);

                parameters.Add("v_IdentityNumber", employee.IdentityNumber);

                parameters.Add("v_IdentityIssuedDate", employee.IdentityIssuedDate);

                parameters.Add("v_IdentityIssuedPlace", employee.IdentityIssuedPlace);

                parameters.Add("v_Address", employee.Address);

                parameters.Add("v_Mobile", employee.Mobile);

                parameters.Add("v_LandlinePhone", employee.LandlinePhone);

                parameters.Add("v_Email", employee.Email);

                parameters.Add("v_BankAccount", employee.BankAccount);

                parameters.Add("v_BankName", employee.BankName);

                parameters.Add("v_BankBranch", employee.BankBranch);

                parameters.Add("v_DepartmentID", "11452b0c-768e-5ff7-0d63-eeb1d8ed8cef");

                parameters.Add("v_DepartmentName", employee.DepartmentName);

                parameters.Add("v_PositionName", employee.PositionName);

                parameters.Add("v_IsSupplier", employee.IsSupplier);

                parameters.Add("v_IsCustomer", employee.IsCustomer);

                parameters.Add("v_ModifiedBy", "Hứa Minh Hiếu");

                parameters.Add("v_ModifiedDate", DateTime.Now);

                //employee.EmployeeID = employeeID;

                //thực hiện gọi vào DB
                var employeeUpdate = mySQlConnection.Execute(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //xử lý dữ liệu trả về
                if (employeeUpdate > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                //  return StatusCode(StatusCodes.Status200OK, employeeUpdate);

            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, exception);

            }

        }
        #endregion

        #region Delete Employee
        /// <summary>
        /// API xóa mới một nhân viên 
        /// </summary>
        /// Created by: HMHieu(18/09/2022)
        /// <param name="employeeID"></param>
        /// <returns>Id của nhân viên vừa xóa </returns>
        [HttpDelete("{employeeID}")]
        public IActionResult Employee([FromRoute] Guid employeeID)
        {

            try
            {
                var mySQLConnection = new MySqlConnection(connectionString);

                var procName = "Proc_employee_DeleteEmployeeByID";

                var parameters = new DynamicParameters();

                parameters.Add("v_EmployeeID", employeeID);

                var employeeDelete = mySQLConnection.Execute(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (employeeDelete > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, exception);

            }

        }
        #endregion

        #region Delete Mutiple Employees 

        /// <summary>
        /// API  xóa nhiều
        /// </summary>
        /// Created by: HMHieu(18/09/2022)
        /// <param name="listdelete"></param>
        /// <returns></returns>
        [HttpPost("DeleteMutiple")]
        public IActionResult DeleteListEployee([FromBody] List<string> listdelete)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, exception);

            }

        }

        #endregion
    }
}
