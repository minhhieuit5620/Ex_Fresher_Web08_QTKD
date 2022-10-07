using Dapper;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Enums;
using Ex_HMH_DL.BaseDL;
using Ex_QTKD_API.Entities;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ex_HMH_DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
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
                vWhere = $" EmployeeCode LIKE \'%{search}%\' OR EmployeeName LIKE \'%{search}%\' OR Mobile LIKE '%{search}%' OR LandlinePhone LIKE '%{search}%'  ";
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
        /// Thay đổi trạng thái người dùng
        /// </summary>
        /// <returns>Guid nhân viên</returns>
        /// CreatedBy: Hứa Minh Hiếu
        public Guid ChangeStatus(Status statusEmployee,Guid EmployeeID)
        {
            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                var procName = "Proc_employee_ChangeStatus";

                var parameter = new DynamicParameters();

                parameter.Add("v_EmployeeID", EmployeeID);

                parameter.Add("v_Status",statusEmployee);

                var numberOfAffecttedRows = mySQLConnection.Execute(procName,parameter, commandType: System.Data.CommandType.StoredProcedure);

                if (numberOfAffecttedRows > 0)
                {
                    return EmployeeID;
                }
                else  //Thát bại
                {
                    return Guid.Empty;
                }
            }
        }
       
        /// <summary>
        /// Tạo mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// CreatedBy: Hứa Minh Hiếu
        public string NewEmployeeCode()
        {
            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                var procName = "Proc_employee_GetMaxEmployeeCode";

                var maxCode = mySQLConnection.QueryFirstOrDefault<string>(procName, commandType: System.Data.CommandType.StoredProcedure);
                
                return maxCode.ToString();
            }
        }

        /// <summary>
        /// Xóa nhiều nhân viên cùng lúc
        /// </summary>
        /// <param name="listEmployeeID"></param>
        /// <returns>Số bản ghi đã xóa</returns>
        public int DeleteMultiple(List<Guid> listEmployeeID)
        {
            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                if (mySQLConnection.State != System.Data.ConnectionState.Open)
                {
                    mySQLConnection.Open();
                }
                //mySQLConnection.Open();
                using (var tran = mySQLConnection.BeginTransaction())
                {
                    var procName = "Proc_employee_DeleteMultiple";

                    string listEmployee = "";

                    foreach (var employee in listEmployeeID)
                    {
                        listEmployee += employee.ToString() + ",";
                    }

                    var a = listEmployee.Substring(0, listEmployee.Length - 1);

                    var parameters = new DynamicParameters();

                    parameters.Add("v_employeeID", a);

                    var numberOfAffecttedRows = mySQLConnection.Execute(procName, parameters,transaction:tran, commandType: System.Data.CommandType.StoredProcedure);

                    if (numberOfAffecttedRows > 0)
                    {
                        tran.Commit();
                        return numberOfAffecttedRows;
                    }
                    else  //Thát bại
                    {
                        tran.Rollback();
                        return 0;
                    }
                }
            }
        }
    
    }
}
