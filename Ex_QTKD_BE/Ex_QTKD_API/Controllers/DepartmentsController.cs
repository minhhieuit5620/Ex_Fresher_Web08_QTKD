using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Ex_QTKD_API.Controllers
{

    /// <summary>
    /// Các API của bảng Phòng ban
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        ///  khởi tạo kết nối đến DB
        /// </summary>

        string connectionString = "Server=localhost;Port=3306;Database=misa.qtkd.ex.hmhieu;Uid=root;Pwd=Hieu@5620;";
        [HttpGet]
        [Route("get-all-department")]
        public IActionResult AllDeparment()
        {
            try
            {
                var mySqlConnection = new MySqlConnection(connectionString);

                var procName = "Proc_department_GetAllDeparment";
                // 
                var deparmentList = mySqlConnection.Query(procName, commandType: System.Data.CommandType.StoredProcedure);

                if (deparmentList != null)
                {
                    return StatusCode(StatusCodes.Status200OK, deparmentList);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}
