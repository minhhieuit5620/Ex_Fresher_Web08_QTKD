

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Misa.AMIS.BL.BaseBL;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Enums;
using Misa.AMIS.Common;
using Misa.AMIS.Common.Handle;
using System;
using NLog.Fluent;
using NLog;
using Microsoft.AspNetCore.Authorization;

namespace Misa.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
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
        public IActionResult GetAllRecord()
        {
            try
            {

                var records = _baseBL.GetAllRecords();
                //dữ liệu hợp lệ
                if(records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);
                }
                else
                {
                    //Trả về mã lỗi cho hàm handle và status cho client
                    return StatusCode(StatusCodes.Status400BadRequest , HandleError.HandleErrorResult(ErrorCode.InvalidInput));
                }                             
            }
            catch (Exception exception)
            {
                //lưu ex vào nLog
                HandleError.HandleNlog(exception);
                //Trả về mã lỗi cho hàm handle và status cho client
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
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
        public IActionResult GetRecordByID([FromRoute] Guid ID)
        {
            try
            {
                var record = _baseBL.GetRecordByID(ID);

                //dữ liệu hợp lệ
                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }
                else
                {
                    //Trả về mã lỗi cho hàm handle và status cho client
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput));
                }
            }
            catch (Exception exception)
            {
                HandleError.HandleNlog(exception);
                //Trả về mã lỗi cho hàm handle và status cho client
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }

        }


        /// <summary>
        /// API thêm mới một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>mã bản ghi</returns>
        /// Created by: HMHieu(18/09/2022)		result.Success	true	bool

        [HttpPost]
        public IActionResult InsertRecord([FromBody] T record)
        {
            try
            {
                var result = _baseBL.Insert(record);

                //dữ liệu hợp lệ 
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                else
                {   //validate lỗi                 
                    //trả về theo các message được định nghĩa sẵn khi validate ở BL                                    
                    return StatusCode(StatusCodes.Status400BadRequest,result);  
                }
            }

            catch (MySqlException mySqlException)// Bắt các ex nếu insert gặp phải lỗi từ DB
            {              
                //trả về mã lỗi 400 vì các ex của MYSQL là do người dùng thao tác gây lên
                return StatusCode(StatusCodes.Status400BadRequest,HandleError.HandleErrorResult(ErrorCode.InsertFailed));
            }

            catch (Exception exception)
            {
                HandleError.HandleNlog(exception);
                //Trả về mã lỗi cho hàm handle và status cho client
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
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
        public IActionResult PutRecord([FromRoute] Guid ID, [FromBody] T record)
        {
            try
            {
                //var recordUpdate = _baseBL.Update(ID, record);

                var result = _baseBL.Update(ID,record);

                if (result.Success)//thành công
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else//thất bại
                { //Trả về mã lỗi cho hàm handle và status cho client
                    return StatusCode(StatusCodes.Status400BadRequest,result);                  

                }
            }
            catch (MySqlException mySqlException)
            {
                //Trả về mã lỗi cho hàm handle và status cho client
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InsertFailed));
                //trả về mã 400 vì lỗi ở MySQL là do người dùng gây lên
            }
            catch (Exception exception)
            {
                HandleError.HandleNlog(exception);
                //Trả về mã lỗi cho hàm handle và status cho client
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }

        }

        /// <summary>
        /// API xóa một bản ghi
        /// </summary>
        /// Created by: HMHieu(18/09/2022)
        /// <param name="ID"></param>
        /// <returns>Id của bản ghi vừa xóa </returns>
        [HttpDelete("{ID}")]
        public IActionResult DeleteRecord([FromRoute] Guid ID)
        {           
            try
            {
                var recordDelete = _baseBL.Delete(ID);

                //xử lý dữ liệu trả về
                if (recordDelete.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, recordDelete);
                }
                else
                {   //trả về status lỗi và gửi mã lỗi về hàm handle
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleErrorResult(ErrorCode.InvalidInput));
                    
                }
            }
            catch (Exception exception)
            {

                HandleError.HandleNlog(exception);//lưu ex vào nLog
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.HandleErrorResult(ErrorCode.Exception));
            }
        }

        /// <summary>
        /// Hàm ghi lại dữ liệu vào nLog
        /// </summary>
        /// <param name="exception"></param>
        /// CreatedBy: HMHieu
        //public static void HandleNlog(Exception exception)
        //{
        //    Logger logger = LogManager.GetLogger("fileLogger");
        //    logger.Error(exception.Message);
        //    //_logger.LogError("Error", exception.Message);
        //    ////_logger.LogTrace("Trace", exception.StackTrace);
        //}
        
        /// <summary>
        /// Hàm xử lý các lỗi có thể bao quát
        /// </summary>
        /// <param name="typeError"></param>
        /// <returns>mã lỗi quy định, message</returns>
        //public static ErrorResult  HandleError(ErrorCode typeError)
        //{
        //    string devMsg = "";//message cho dev
        //    string userMsg = "";//message cho người dùng
        //    var moreInfo = Resource.Moreinfo_InsertFailed;///link  dẫn lỗi
        //    ErrorCode errorCode = ErrorCode.Exception;//Mã lỗi được quy định để handle

        //    switch (typeError)
        //    { 
        //        case ErrorCode.Exception:// mã lỗi và messaga trả về khi gặp ex
        //            devMsg = Resource.DevMsg_Exception;
        //            userMsg = Resource.UserMsg_Exception;
        //            moreInfo = Resource.Moreinfo_InsertFailed;
        //            errorCode = ErrorCode.Exception;
        //            break;

        //        case ErrorCode.InvalidInput: //mã lỗi và message trả về khi validate lỗi
        //            devMsg = Resource.DevMsg_validateFailed;
        //            userMsg = Resource.UserMsg_validateFailed;
        //            moreInfo = Resource.Moreinfo_InsertFailed;
        //            errorCode = ErrorCode.InvalidInput;
        //            break;

        //        case ErrorCode.InsertFailed:// mã lỗi và message trả về khi không insert thành công
        //            devMsg = Resource.DevMsg_InsertFailed;
        //            userMsg = Resource.UserMsg_InsertFailed;
        //            moreInfo = Resource.Moreinfo_InsertFailed;
        //            errorCode = ErrorCode.InsertFailed;
        //            break;

        //        case ErrorCode.DuplicateCode:// mã lỗi và message trả về khi mã nhân viên trùng
        //            devMsg = Resource.DevMsg_DuplicateCode;
        //            userMsg = Resource.UserMsg_DuplicateCode;
        //            moreInfo = Resource.Moreinfo_InsertFailed;
        //            errorCode = ErrorCode.DuplicateCode;
        //            break;

        //        case ErrorCode.NotExitsFile:// mã lỗi và message trả không tồn tại file
        //            devMsg = Resource.DevMsg_InsertFailed;
        //            userMsg = Resource.FileImportNull;
        //            moreInfo = Resource.Moreinfo_InsertFailed;
        //            errorCode = ErrorCode.NotExitsFile;
        //            break;
        //    }

        //    return new ErrorResult
        //    (   errorCode,
        //        devMsg,
        //        userMsg,
        //        moreInfo
        //    );
                
           
        //}
       

        #endregion
    }
}
