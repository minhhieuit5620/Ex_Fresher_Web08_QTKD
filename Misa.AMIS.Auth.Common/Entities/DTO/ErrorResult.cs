using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Auth.Common.Entities.DTO
{
    public class ErrorResult
    {

        public ErrorResult(ErrorCode errorCode, string? msg)
        {
            ErrorCode = errorCode;
            Msg = msg;          
        }
        /// <summary>
        /// Các mã lỗi 
        /// </summary>
        public ErrorCode ErrorCode { get; set; }
        /// <summary>
        /// Lỗi thông báo  
        /// </summary>
        public string? Msg { get; set; }
    }
}
