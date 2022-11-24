namespace Misa.AMIS.Common.Enums
{
    /// <summary>
    /// Các mã lỗi
    /// </summary>
    public enum ErrorCode
    {

        /// <summary>
        /// Exception
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Trùng mã nhân viên
        /// </summary>
        DuplicateCode = 2,


        /// <summary>
        /// Insert thất bại
        /// </summary>
        InsertFailed = 3,

        /// <summary>
        /// Dữ liệu đầu vào bị lỗi
        /// </summary>
        InvalidInput = 4,

        /// <summary>
        /// Xóa không thành công 
        /// </summary>
        DeleteFailed = 4,

        /// <summary>
        /// Mã nhân viên không đúng địng dạng
        /// </summary>
        InvalidEmployeeCode = 5,

        /// <summary>
        /// Không tồn tại file
        /// </summary>
        NotExitsFile= 6,

       


    }
}
