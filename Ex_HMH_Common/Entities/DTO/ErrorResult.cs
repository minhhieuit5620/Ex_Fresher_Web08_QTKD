using Ex_HMH_Common.Enums;

namespace Ex_QTKD_API.Entities
{
    public class ErrorResult
    {
        public ErrorResult(ErrorCode errorCode, string devMsg, string userMsg, string moreInfo, string? traceId=null)
        {
            ErrorCode = errorCode;
            DevMsg = devMsg;
            UserMsg = userMsg;
            MoreInfo = moreInfo;
            TraceId = traceId;
        }
        /// <summary>
        /// Các mã lỗi 
        /// </summary>
        public ErrorCode ErrorCode { get; set; }
        /// <summary>
        /// Lỗi thông báo  cho Dev
        /// </summary>
        public string DevMsg { get; set; }
        /// <summary>
        /// Lỗi thông báo cho  người dùng
        /// </summary>
        public string UserMsg { get; set; }
        /// <summary>
        /// Thông tin chi tiết hơn về vấn đề.
        /// </summary>
        public string MoreInfo { get; set; }
        /// <summary>
        /// Mã để tra cứu thông tin log
        /// </summary>
        public string? TraceId { get; set; }
    }
   
}
