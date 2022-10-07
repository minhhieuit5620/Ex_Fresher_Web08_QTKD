 
using Ex_HMH_BL;
using Ex_HMH_BL.BaseBL;
using Ex_HMH_BL.DepartmentBL;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Enums;
using Ex_HMH_Common;
using Ex_QTKD_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Ex_HMH_QTKD_08_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Feild

        private readonly ILogger<T> _logger;

        private IBaseBL<T> _baseBL;
       
        #endregion

        #region Contructor

        public BasesController(ILogger<T> logger, IBaseBL<T> baseBL)
        {
            _logger = logger;
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API lấy danh sách tất cả các bản ghi
        /// </summary>
        /// <returns>Danh sách tất cả các bản ghi của một bảng</returns>
        /// CreatedBy Hứa Minh Hiếu(30-09-2022)
        [HttpGet]
        [Route("")]
        public IActionResult AllRecord()
        {
            try
            {
                var records = _baseBL.GetAllRecords();

                return StatusCode(StatusCodes.Status200OK, records);
                
            }
            catch (Exception exception)
            {
                _logger.LogError("Error", exception.Message);
                _logger.LogTrace("Trace", exception.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// API  Filter bản ghi
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sort"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns>
        /// + Danh sách bản ghi thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số bản ghi thỏa mã các điều kiện lọc
        /// </returns>
        /// Created by: HMHieu(18/09/2022)
        //[HttpGet("filter")]
        //public IActionResult FilterRecord(
        //    [FromQuery] string? search,
        //    [FromQuery] string? sort,
        //    [FromQuery] int pageIndex,
        //    [FromQuery] int pageSize)

        //{
        //    try
        //    {
        //        var result = _baseBL.Filter(search, sort, pageIndex, pageSize);
        //        if (result != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new PagingData<T>()
        //            {
        //                Data = result.Data.ToList(),

        //                TotalCount = result.TotalCount,

        //                TotalPage = result.TotalCount / pageSize + 1,

        //            });
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError("Error", exception.Message);
        //        _logger.LogTrace("Trace", exception.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }

        //}

        /// <summary>
        /// API lấy một  bản ghi
        /// </summary>      
        /// <param name="ID"></param>
        /// <returns>dữ liệu bản ghi tương ứng với mã đã nhập</returns>
        /// Created by: HMHieu(30/09/2022)
        [HttpGet("{ID}")]
        public IActionResult RecordByID([FromRoute] Guid ID)
        {
            try
            {
                var record = _baseBL.RecordByID(ID);

                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
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
                _logger.LogError("Error", exception.Message);
                _logger.LogTrace("Trace", exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// API thêm mới một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>mã bản ghi</returns>
        /// Created by: HMHieu(18/09/2022)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] T record)
        {
            try
            {
                var result = _baseBL.Insert(record);

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
            catch (MySqlException mySqlException)
            {

                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry) // trùng mã nhân viên
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                                            ErrorCode.DuplicateCode,
                                            Resource.DevMsg_DuplicateCode,
                                            Resource.UserMsg_DuplicateCode,
                                            Resource.Moreinfo_InsertFailed,
                                            HttpContext.TraceIdentifier
                                        ));
                }
                return StatusCode(StatusCodes.Status500InternalServerError,
                                   new ErrorResult(
                                           ErrorCode.Exception,
                                           Resource.DevMsg_Exception,
                                           Resource.UserMsg_Exception,
                                           Resource.Moreinfo_InsertFailed,
                                           HttpContext.TraceIdentifier
                                       )
                                );
            }


            catch (Exception ex)
            {
                _logger.LogError("Error", ex.Message);
                _logger.LogTrace("Trace", ex.Message);

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

        /// <summary>
        /// API sửa một bản ghi
        /// </summary>
        /// Created by: HMHieu(18/09/2022)
        /// <param name="ID"></param>
        /// <param name="record"></param>
        /// <returns>ID bản ghi vừa sửa </returns>
        [HttpPut("{ID}")]
        public IActionResult Record([FromRoute] Guid ID, [FromBody] T record)
        {
            try
            {
                var recordUpdate = _baseBL.Update(ID, record);

                //xử lý dữ liệu trả về
                if (recordUpdate.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, recordUpdate.Data);
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

            catch (MySqlException mySqlException)
            {

                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry) // trùng mã nhân viên
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                                            ErrorCode.DuplicateCode,
                                            Resource.DevMsg_DuplicateCode,
                                            Resource.UserMsg_DuplicateCode,
                                            Resource.Moreinfo_InsertFailed,
                                            HttpContext.TraceIdentifier
                                        ));
                }
                return StatusCode(StatusCodes.Status500InternalServerError,
                                   new ErrorResult(
                                           ErrorCode.Exception,
                                           Resource.DevMsg_Exception,
                                           Resource.UserMsg_Exception,
                                           Resource.Moreinfo_InsertFailed,
                                           HttpContext.TraceIdentifier
                                       )
                                );
            }

            catch (Exception exception)
            {
                _logger.LogError("Error", exception.Message);
                _logger.LogTrace("Trace", exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);

            }

        }

        /// <summary>
        /// API xóa một bản ghi
        /// </summary>
        /// Created by: HMHieu(18/09/2022)
        /// <param name="ID"></param>
        /// <returns>Id của bản ghi vừa xóa </returns>
        [HttpDelete("{ID}")]
        public IActionResult Record([FromRoute] Guid ID)
        {
            try
            {
                var recordDelete = _baseBL.Delete(ID);

                if (recordDelete != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, ID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Error", exception.Message);
                _logger.LogTrace("Trace", exception.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
