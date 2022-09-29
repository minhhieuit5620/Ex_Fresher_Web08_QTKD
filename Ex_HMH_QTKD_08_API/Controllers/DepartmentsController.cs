using Ex_HMH_BL;
using Ex_HMH_BL.DepartmentBL;
using Ex_HMH_Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;

namespace Ex_HMH_QTKD_08_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {

        #region Feild
        private readonly ILogger<DepartmentsController> _logger;

        private IDepartmentBL _departmentBL;

        #endregion

        #region Contructor

        public DepartmentsController(ILogger<DepartmentsController> logger, IDepartmentBL department)
        {
            _logger = logger;
            _departmentBL = department;
        }

        #endregion

        #region Method

        [HttpGet]
        [Route("")]
        public IActionResult AllDeparment()
        {
            try
            {
                var department = _departmentBL.AllDepartment();

                return StatusCode(StatusCodes.Status200OK, department);
              

            }
            catch (Exception exception)
            {
                _logger.LogError("Error", exception);
                _logger.LogTrace("Trace", exception);

                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        #endregion
    }
}
