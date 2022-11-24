using Microsoft.Extensions.Logging;
using Misa.AMIS.Auth.Common.Entities.DTO;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Auth.Common.HandleErrorAuth
{
    public class HandleErrorAuth
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
        public static ErrorResult HandleErrorAuthResult(ErrorCode typeError)
        {
            string Msg = "";//message cho dev          
            ErrorCode errorCode = ErrorCode.exception;//Mã lỗi được quy định để handle

            switch (typeError)
            {
                case ErrorCode.exception:// mã lỗi và messaga trả về khi gặp ex
                    Msg = Resource.Exception;                  
                    errorCode = ErrorCode.exception;
                    break;
                case ErrorCode.duplicateUserName:// mã lỗi và messaga trả về khi gặp ex
                    Msg = Resource.UsernameExitsed;
                    errorCode = ErrorCode.duplicateUserName;
                    break;
                case ErrorCode.regiterFailed:// mã lỗi và messaga trả về khi gặp ex
                    Msg = Resource.RegisterFailed;
                    errorCode = ErrorCode.regiterFailed;
                    break;

                case ErrorCode.loginFailed: //mã lỗi và message trả về khi validate lỗi
                    Msg = Resource.LoginFailed;
                    errorCode = ErrorCode.loginFailed;
                    break;

                case ErrorCode.confirmPassNotMatch: //mã lỗi và message trả về khi validate lỗi
                    Msg = Resource.ErrorConfirmPass;
                    errorCode = ErrorCode.confirmPassNotMatch;
                    break;

                case ErrorCode.EmptyModel:// mã lỗi và message trả về khi mã nhân viên trùng
                    Msg = Resource.Empty;
                    errorCode = ErrorCode.EmptyModel;
                    break;
                case ErrorCode.validateFeiled:// mã lỗi và message trả về khi mã nhân viên trùng
                    Msg = Resource.Empty;
                    errorCode = ErrorCode.validateFeiled;
                    break;              

            }

            return new ErrorResult
            (errorCode,
                Msg              
            );


        }
    }
}
