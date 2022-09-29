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

namespace Ex_HMH_QTKD_08_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        #region Feild
        private readonly ILogger<EmployeesController> _logger;
        private IEmployeeBL _employeeBL;

        #endregion

        #region Contructor

        public EmployeesController (ILogger<EmployeesController> logger, IEmployeeBL employeeBL)
        {
            _logger = logger;
            _employeeBL = employeeBL;
        }

        #endregion

        #region Method

        #region Get FilterEmployee 
        /// <summary>
        /// API lấy Filter nhân viên 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="departmentID"></param>
        /// <param name="sort"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns>
        /// + Danh sách nhân viên thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số nhân viên thỏa mã các điều kiện lọc
        /// </returns>
        /// Created by: HMHieu(18/09/2022)
        /// 
        [HttpGet("filter")]
        public IActionResult FilterEmployee(
            [FromQuery] string? search,
            [FromQuery] string? sort,
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize)

        {
            try
            {
                var result = _employeeBL.FilterEmployee(search,sort,pageIndex,pageSize);
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Employee>()
                    {
                        Data = result.Data.ToList(),

                        TotalCount = result.TotalCount,

                        TotalPage = result.TotalCount / pageSize + 1,

                    });                   
                }                
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Error", exception);
                _logger.LogTrace("Trace", exception);
                return StatusCode(StatusCodes.Status400BadRequest, exception);
            }

        }
        #endregion

        #region  Get EmployeeByID
        /// <summary>
        /// API lấy một  nhân viên 
        /// </summary>      
        /// <param name="employeeID"></param>
        /// <returns>dữ liệu nhân viên tương ứng với mã đã nhập</returns>
        /// Created by: HMHieu(18/09/2022)
        [HttpGet("{employeeID}")]
        public IActionResult EmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                var employee = _employeeBL.EmployeeByID(employeeID);

                if(employee != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employee);
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
            catch (Exception exception)
            {              
                _logger.LogError("Error",exception);
                _logger.LogTrace("Trace", exception);
                return StatusCode(StatusCodes.Status400BadRequest, exception);
            }

        }
        #endregion


        #region  POST Employee
        /// <summary>
        /// API thêm mới một nhân viên 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>mã nhân viên</returns>
        /// <returns>Mã nhân viên tự động tăng</returns>
        /// Created by: HMHieu(18/09/2022)
        [HttpPost]
        public IActionResult Employee([FromBody] Employee employee)
        {
            try
            {
                var result = _employeeBL.InsertEmployee(employee);

                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                                       new ErrorResult(
                                           ErrorCode.InvalidInput,
                                           Resource.DevMsg_validateFailed,
                                           Resource.UserMsg_validateFailed,
                                           Resource.Moreinfo_InsertFailed,
                                           HttpContext.TraceIdentifier
                                      ));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                _logger.LogTrace("Trace", ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new ErrorResult(
                                             ErrorCode.Exception,
                                             Resource.DevMsg_Exception,
                                             Resource.UserMsg_Exception,
                                             Resource.Moreinfo_InsertFailed,
                                             HttpContext.TraceIdentifier
                                         ));
            }
        }
        #endregion

        #region PUT Employee 
        /// <summary>
        /// API sửa một nhân viên 
        /// </summary>
        /// Created by: HMHieu(18/09/2022)
        /// <param name="employeeID"></param>
        /// <param name="employee"></param>
        /// <returns>nhân viên vừa sửa </returns>
        [HttpPut("{employeeID}")]
        public IActionResult Employee([FromRoute] Guid employeeID, [FromBody] Employee employee)
        {

            try
            {
                var employeeUpdate = _employeeBL.UpdateEmployee(employeeID,employee);

                //xử lý dữ liệu trả về
                if (employeeUpdate.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeUpdate.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                                       new ErrorResult(
                                           ErrorCode.InvalidInput,
                                           Resource.DevMsg_validateFailed,
                                           Resource.UserMsg_validateFailed,
                                           Resource.Moreinfo_InsertFailed,
                                           HttpContext.TraceIdentifier
                                      ));
                }           
            }
            catch (Exception exception)
            {
                _logger.LogError("Error", exception);
                _logger.LogTrace("Trace", exception);
                return StatusCode(StatusCodes.Status400BadRequest, exception);

            }

        }
        #endregion

        #region Delete Employee
        /// <summary>
        /// API xóa mới một nhân viên 
        /// </summary>
        /// Created by: HMHieu(18/09/2022)
        /// <param name="employeeID"></param>
        /// <returns>Id của nhân viên vừa xóa </returns>
        [HttpDelete("{employeeID}")]
        public IActionResult Employee([FromRoute] Guid employeeID)
        {
            try
            {
                var employeeDelete = _employeeBL.DeleteEmployee(employeeID);

                if (employeeDelete!=Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Error", exception);
                _logger.LogTrace("Trace", exception);

                return StatusCode(StatusCodes.Status400BadRequest, exception);
            }
        }
        #endregion
        #endregion


    }
}
