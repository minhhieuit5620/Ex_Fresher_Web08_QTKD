using Dapper;
using Ex_HMH_Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ex_HMH_DL
{
    internal interface IEmployeeDL
    {
        public IEnumerable<Employee> GetAnyTable(string nameTable)
        {
            //Khởi tạo kết nối tời DB MySQL
            string connect=DataContext.MySqlConnectionString;

            string connectionString = "Server=localhost;Port=3306;Database=misa.qtkd.ex.hmhieu;Uid=root;Pwd=Hieu@5620;";
            var mysqlConnection = new MySqlConnection(connectionString);

            string StoredProc = "Proc_employee_GetAllTable";

            //Chuẩn bị tham số đầu vào cho câu lệnh trên
            var parameters = new DynamicParameters();

            parameters.Add("Table_name", nameTable);

            //Thực hiện gọi vào DB

            var employee = mysqlConnection.Query(StoredProc, parameters, commandType: System.Data.CommandType.StoredProcedure);
            return employee.ToList();
        }

        /// <summary>
        /// API thêm mới một nhân viên 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>mã nhân viên</returns>
        /// <returns>Return về 1 nếu thêm mới thành công, 0 nếu thất bại</returns>
        /// Created by: HMHieu(18/09/2022)

        public int InserEmployee(Employee employee);
    }
        
}
