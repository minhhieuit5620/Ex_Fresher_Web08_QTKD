using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySqlConnector;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Attributes;
using Ex_HMH_Common.Enums;
using Ex_HMH_Common;
using Ex_HMH_BL;
using Ex_QTKD_API.Entities;
using System;
using Ex_HMH_DL;
using NLog;
using Ex_HMH_BL.BaseBL;
using Microsoft.Extensions.Logging;

namespace Ex_HMH_QTKD_08_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BasesController<Employee>
    {

        #region Feild

        private readonly ILogger<Employee> _logger;

        private IEmployeeBL _employeeBL;

        #endregion

        #region Contructor               

        public EmployeesController(ILogger<Employee> logger, IEmployeeBL employeeBL) : base(logger, employeeBL)
        {
            _employeeBL = employeeBL;
            _logger = logger;
        }

        #endregion

        #region Method

        /// <summary>
        /// API tạo mã nhân viên mới
        /// </summary>
        /// <returns>mã hân viên mới</returns>
        /// CreatedBy: Hứa minh hiếu (24-09-2022)
        [HttpGet("new-code-employee")]
        public IActionResult newCodeEmployee()
        {
            try
            {
                var maxCode = _employeeBL.NewEmployeeCode();

                if (maxCode != "")
                {


                    return StatusCode(StatusCodes.Status200OK, maxCode);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        ErrorCode.InvalidEmployeeCode,
                        Resource.DevMsg_DuplicateCode,
                        Resource.UserMsg_DuplicateCode,
                        Resource.Moreinfo_InsertFailed
                        ));
                }



            }
            catch (Exception ex)
            {

                _logger.LogError("Error", ex);
                _logger.LogTrace("Trace", ex);

                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

        /// <summary>
        /// API thay đổi trạng thái làm việc của nhân viên
        /// </summary>
        /// <param name="statusEmployee"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>ID của nhân viên thay đổi</returns>
        [HttpPut("change-status")]
        public IActionResult ChangeStatus(Status statusEmployee, Guid EmployeeID)
        {
            try
            {
                var employeeChange = _employeeBL.ChangeStatus(statusEmployee, EmployeeID);

                if (employeeChange != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeChange);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        ErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_Exception,
                        Resource.Moreinfo_InsertFailed
                        ));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                _logger.LogTrace("Trace", ex);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion


    }
}
