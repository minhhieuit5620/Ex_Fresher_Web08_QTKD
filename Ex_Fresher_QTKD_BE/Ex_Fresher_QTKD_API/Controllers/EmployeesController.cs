using Dapper;
using Ex_Fresher_QTKD_API.Entities;
using Ex_Fresher_QTKD_API.Entities.DTO;
using Ex_Fresher_QTKD_API.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Data;
using System.Net.WebSockets;

namespace Ex_Fresher_QTKD_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //Khởi tạo kết nối tời DB MySQL
        string connectionString = "Server=localhost;Port=3306;Database=misa.qtkd.ex.hmhieu;Uid=root;Pwd=Hieu@5620;";

        #region get any table
        [HttpGet]
        [Route("getTable/{nameTable}")]
        public IActionResult Getakakak([FromRoute] string nameTable)
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
            [FromQuery] int? offset = 1, 
            [FromQuery] int? limit = 10)
        {           
            try
            {
                var mySQLConnection=new MySqlConnection(connectionString);
                // Khởi tạo kết nối đến DB và gọi proc
                var procName = "Proc_employee_GetDataFilterPaging";

                //Chuẩn bị tham số đầu vào cho Proc

                var parameters = new DynamicParameters();
                parameters.Add("v_Offset", offset);

                parameters.Add("v_Limit", limit);
                
                parameters.Add("v_Sort", sort);

                string vWhere = "";

                if (search != null)
                {
                    vWhere = $" EmployeeCode LIKE \'%{search}%\' OR FullName LIKE \'%{search}%\'  ";
                }
                parameters.Add("v_Where", vWhere);



                // Thực hiện gọi vào DB
                var results = mySQLConnection.QueryMultiple(procName,parameters,commandType:System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về 

                if (results != null)
                {
                    var employeeList = results.Read<Employee>();

                    var totalCount=results.Read<int>().Single();

                    return StatusCode(StatusCodes.Status200OK, new PagingData<Employee>()
                    {
                        Data = employeeList.ToList(),

                        TotalCount=totalCount

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
        /// <returns></returns>
        /// Created by: HMHieu(18/09/2022)
        [HttpGet("{employeeID}")]
        public IActionResult GetEmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                // tạo kết nối đến Db và gán tên store
                var mySQLConnection = new MySqlConnection(connectionString);

                var procname = "Proc_employee_GetEmployeeByID";

                // chuẩn bị các tham số truyền vào proc

                var parameters = new DynamicParameters();

                parameters.Add("$EmployeeID",employeeID);

                //thưc hiện gọi vào DB 
                var employee=mySQLConnection.QueryFirstOrDefault(procname,parameters,commandType:System.Data.CommandType.StoredProcedure);

                return StatusCode(StatusCodes.Status200OK,employee);
                
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, exception);
            }

        }
        #endregion

        #region  POST Employee
        /// <summary>
        /// API thêm mới một nhân viên 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Mã nhân viên tự động tăng</returns>
        /// Created by: HMHieu(18/09/2022)
        [HttpPost]
        public IActionResult InserEmployee([FromBody] Employee employee)
        {
            try
            {
                //Khởi tạo kết nối tới DB MySQL 
                string connectionString = "Server=localhost;Port=3306;Database=misa.qtkd.ex.hmhieu;Uid=root;Pwd=Hieu@5620;";
                var mysqlConnection = new MySqlConnection(connectionString);

                //Khai báo tên Proc

                string nameProc = "Proc_employee_InsertOne";
                var guid = Guid.NewGuid();
                // chuẩn bị tham số đầu vào cho proc
                var parameters = new DynamicParameters();
                parameters.Add("$EmployeeID", guid);

                parameters.Add("$EmployeeCode", employee.EmployeeCode);

                parameters.Add("$FullName", employee.FullName);

                parameters.Add("$Gender",employee.Gender);

                parameters.Add("$DateOfBirth", employee.DateOfBirth);

                parameters.Add("$IdentityNumber", employee.IdentityNumber);

                parameters.Add("$IdentityIssuedDate", employee.IdentityIssuedDate);

                parameters.Add("$IdentityIssuedPlace", employee.IdentityIssuedPlace);

                parameters.Add("$Address", employee.Address);

                parameters.Add("$Mobile", employee.Mobile);

                parameters.Add("$LandlinePhone", employee.LandlinePhone);

                parameters.Add("$Email", employee.Email);

                parameters.Add("$BankAccount", employee.BankAccount);

                parameters.Add("$BankName", employee.BankName);

                parameters.Add("$BankBranch", employee.BankBranch);

                parameters.Add("$DepartmentID", "11452b0c-768e-5ff7-0d63-eeb1d8ed8cef");

                parameters.Add("$DepartmentName", employee.DepartmentName);

                parameters.Add("$PositionName", employee.PositionName);

                parameters.Add("$IsCustomer", employee.IsCustomer);

                parameters.Add("$IsSupplier", employee.IsSupplier);       
                
                parameters.Add("$CreatedBy", employee.CreatedBy);

                var numberOfAffecttedRows = mysqlConnection.Execute(nameProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //xử lý dữ liệu trả về 

                if (numberOfAffecttedRows > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, guid);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                //Thành công

                //Thát bại

                //Try catch ex 
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest);
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
        public IActionResult UpdateEmployee([FromRoute] Guid employeeID, [FromBody] Employee employee)
        {

            try
            {

                //tạo connection đến DB
                var mySQlConnection=new MySqlConnection(connectionString);
                var procName = "Proc_employee_UpdateByID";

                //chuẩn bị tham số đầu vào cho proc
                var parameters = new DynamicParameters();
                parameters.Add("$EmployeeID", employee.EmployeeID);

                parameters.Add("$EmployeeCode", employee.EmployeeCode);

                parameters.Add("$FullName", employee.FullName);

                parameters.Add("$Gender", employee.Gender);

                parameters.Add("$DateOfBirth", employee.DateOfBirth);

                parameters.Add("$IdentityNumber", employee.IdentityNumber);

                parameters.Add("$IdentityIssuedDate", employee.IdentityIssuedDate);

                parameters.Add("$IdentityIssuedPlace", employee.IdentityIssuedPlace);

                parameters.Add("$Address", employee.Address);

                parameters.Add("$Mobile", employee.Mobile);

                parameters.Add("$LandlinePhone", employee.LandlinePhone);

                parameters.Add("$Email", employee.Email);

                parameters.Add("$BankAccount", employee.BankAccount);

                parameters.Add("$BankName", employee.BankName);

                parameters.Add("$BankBranch", employee.BankBranch);

                parameters.Add("$DepartmentID", "11452b0c-768e-5ff7-0d63-eeb1d8ed8cef");

                parameters.Add("$DepartmentName", employee.DepartmentName);

                parameters.Add("$PositionName", employee.PositionName);

                parameters.Add("$IsSupplier", employee.IsSupplier);

                parameters.Add("$IsCustomer", employee.IsCustomer);

                parameters.Add("$ModifiedBy", employee.ModifiedBy);
              
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
        public IActionResult DeleteEmployee([FromRoute] Guid employeeID)
        {

            try
            {
                var mySQLConnection = new MySqlConnection(connectionString);

                var procName = "Proc_employee_DeleteEmployeeByID";

                var parameters = new DynamicParameters();

                parameters.Add("$EmployeeID",employeeID);

                var employeeDelete=mySQLConnection.Execute(procName,parameters,commandType: System.Data.CommandType.StoredProcedure);

                if(employeeDelete > 0)
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
