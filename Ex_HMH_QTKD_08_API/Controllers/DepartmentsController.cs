using Ex_HMH_BL;
using Ex_HMH_BL.BaseBL;
using Ex_HMH_BL.DepartmentBL;
using Ex_HMH_Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;

namespace Ex_HMH_QTKD_08_API.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BasesController<Department>
    {

        public DepartmentsController(ILogger<Department> Ilogger,IBaseBL<Department> baseBL) : base(Ilogger, baseBL)
        {
        }

        #region Method
       
        #endregion
    }
}
