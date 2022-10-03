using Dapper;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Enums;
using Ex_QTKD_API.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ex_HMH_DL
{
    public interface IEmployeeDL: IBaseDL<Employee>
    {
        public string NewEmployeeCode();

        public Guid ChangeStatus(Status statusEmployee, Guid EmployeeID);
    }

}
