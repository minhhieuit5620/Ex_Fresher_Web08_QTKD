using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Enums;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Common.Handle
{
    public class HandleError
    {
        /// <summary>
        /// Hàm ghi lại dữ liệu vào nLog
        /// </summary>
        /// <param name="exception"></param>
        /// CreatedBy: HMHieu
        public static void HandleNlog(Exception exception)
        {
            Logger logger = LogManager.GetLogger("fileLogger");
            logger.Error(exception.Message);
            //_logger.LogError("Error", exception.Message);
            ////_logger.LogTrace("Trace", exception.StackTrace);
        }

        /// <summary>
        /// Hàm xử lý các lỗi có thể bao quát
        /// </summary>
        /// <param name="typeError"></param>
        /// <returns>mã lỗi quy định, message</returns>
        public static ErrorResult HandleErrorResult(ErrorCode typeError)
        {
            string devMsg = "";//message cho dev
            string userMsg = "";//message cho người dùng
            var moreInfo = Resource.Moreinfo_InsertFailed;///link  dẫn lỗi
            ErrorCode errorCode = ErrorCode.Exception;//Mã lỗi được quy định để handle

            switch (typeError)
            {
                case ErrorCode.Exception:// mã lỗi và messaga trả về khi gặp ex
                    devMsg = Resource.DevMsg_Exception;
                    userMsg = Resource.UserMsg_Exception;
                    moreInfo = Resource.Moreinfo_InsertFailed;
                    errorCode = ErrorCode.Exception;
                    break;

                case ErrorCode.InvalidInput: //mã lỗi và message trả về khi validate lỗi
                    devMsg = Resource.DevMsg_validateFailed;
                    userMsg = Resource.UserMsg_validateFailed;
                    moreInfo = Resource.Moreinfo_InsertFailed;
                    errorCode = ErrorCode.InvalidInput;
                    break;

                case ErrorCode.InsertFailed:// mã lỗi và message trả về khi không insert thành công
                    devMsg = Resource.DevMsg_InsertFailed;
                    userMsg = Resource.UserMsg_InsertFailed;
                    moreInfo = Resource.Moreinfo_InsertFailed;
                    errorCode = ErrorCode.InsertFailed;
                    break;

                case ErrorCode.DuplicateCode:// mã lỗi và message trả về khi mã nhân viên trùng
                    devMsg = Resource.DevMsg_DuplicateCode;
                    userMsg = Resource.UserMsg_DuplicateCode;
                    moreInfo = Resource.Moreinfo_InsertFailed;
                    errorCode = ErrorCode.DuplicateCode;
                    break;

                case ErrorCode.NotExitsFile:// mã lỗi và message trả không tồn tại file
                    devMsg = Resource.DevMsg_InsertFailed;
                    userMsg = Resource.FileImportNull;
                    moreInfo = Resource.Moreinfo_InsertFailed;
                    errorCode = ErrorCode.NotExitsFile;
                    break;
            }

            return new ErrorResult
            (errorCode,
                devMsg,
                userMsg,
                moreInfo
            );


        }
    }
}
