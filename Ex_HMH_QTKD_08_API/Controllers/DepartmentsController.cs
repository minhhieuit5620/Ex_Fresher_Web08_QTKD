

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.AMIS.BL.BaseBL;
using Misa.AMIS.Common.Entities;
using MySqlConnector;
using System;

namespace Misa.AMIS.API.Controllers
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
