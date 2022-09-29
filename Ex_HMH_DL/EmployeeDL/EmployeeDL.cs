using Dapper;
using Ex_HMH_Common.Entities;
using Ex_QTKD_API.Entities;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_HMH_DL
{
    public class EmployeeDL : IEmployeeDL
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
        public PagingData<Employee> FilterEmployee(string? search, string? sort, int pageIndex, int pageSize)
        {                    
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

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                // Thực hiện gọi vào DB
                var results = mySQLConnection.QueryMultiple(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var listEmployees = results.Read<Employee>();

                var totalCount = results.Read<int>().Single();


                return new PagingData<Employee>()
                {
                    Data = listEmployees.ToList(),

                    TotalCount = totalCount,

                    TotalPage = totalCount / pageSize + 1,
                };
            }
        }

        /// <summary>
        /// lấy một  nhân viên 
        /// </summary>      
        /// <param name="employeeID"></param>
        /// <returns>dữ liệu nhân viên tương ứng với mã nhân viên đã nhập</returns>
        /// Created by: HMHieu(28/09/2022)
        public Employee EmployeeByID(Guid employeeID)
        {       
            var procname = "Proc_employee_GetEmployeeByID";

            // chuẩn bị các tham số truyền vào proc

            var parameters = new DynamicParameters();

            parameters.Add("v_EmployeeID", employeeID);

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //thưc hiện gọi vào DB 
                var employee = mySQLConnection.QueryFirstOrDefault<Employee>(procname, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return employee;
            }

        }

        /// <summary>
        /// thêm mới một nhân viên 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>mã nhân viên</returns>
        /// Created by: HMHieu(28/09/2022)
        public Guid InsertEmployee(Employee employee)
        {           
            string nameProc = "Proc_employee_InsertOne";

            var employeeID = Guid.NewGuid();

            // chuẩn bị tham số đầu vào cho proc
            var parameters = new DynamicParameters();

            parameters.Add("v_EmployeeID", employeeID);

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

            parameters.Add("v_CreatedBy", "Hứa Minh Hiếu");

            parameters.Add("v_CreatedDate", DateTime.Now);

            parameters.Add("v_ModifiedDate", DateTime.Now);

            var numberOfAffecttedRows = 0;

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                 numberOfAffecttedRows = mySQLConnection.Execute(nameProc, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            if(numberOfAffecttedRows > 0)
            {
                return employeeID;
            }
            else
            {
                return Guid.Empty;
            }          
        }

        /// <summary>
        /// sửa một nhân viên 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="employeeID"></param>
        /// <param name="employee"></param>
        /// <returns>Return về Guid vừa sửa, Guid rỗng nếu thất bại </returns>     
        public Guid UpdateEmployee(Guid employeeID, Employee employee)
        {           
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

            var employeeUpdate = 0;
            //thực hiện gọi vào DB
            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                 employeeUpdate = mySQLConnection.Execute(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            if (employeeUpdate > 0)
            {
                return employeeID;
            }
            else
            {
                return Guid.Empty;
            }

        }

         /// <summary>
        /// xóa một nhân viên 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="employeeID"></param>
        /// <returns>Id của nhân viên vừa xóa </returns>
        public Guid DeleteEmployee(Guid employeeID)
        {              
                var procName = "Proc_employee_DeleteEmployeeByID";

                var parameters = new DynamicParameters();

                parameters.Add("v_EmployeeID", employeeID);

                var employeeDelete = 0;

                using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
                {
                     employeeDelete = mySQLConnection.Execute(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                if (employeeDelete > 0)
                {
                    return employeeID;
                }
                else
                {
                    return Guid.Empty;
                }



        }
    }
}
