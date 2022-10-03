using Ex_HMH_BL.BaseBL;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Enums;
using Ex_QTKD_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_HMH_BL
{
    public interface IEmployeeBL: IBaseBL<Employee>
    {
        public string NewEmployeeCode();

        public Guid ChangeStatus(Status statusEmployee, Guid EmployeeID);
    }
}
