using Dapper;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Enums;
using Ex_HMH_DL.BaseDL;
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
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
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

        public string NewEmployeeCode()
        {
            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                var procName = "Proc_employee_GetMaxEmployeeCode";

                var maxCode = mySQLConnection.QueryFirstOrDefault<string>(procName, commandType: System.Data.CommandType.StoredProcedure);
                
                return maxCode.ToString();
            }
        }



    }
}
