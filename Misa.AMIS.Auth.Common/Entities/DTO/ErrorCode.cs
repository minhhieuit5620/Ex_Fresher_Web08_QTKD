using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Auth.Common.Entities.DTO
{
    public enum ErrorCode
    {
        /// <summary>
        /// lỗi gặp exception
        /// </summary>
        exception=1,

        /// <summary>
        /// lỗi gặp khi thông tin đăng ký người dùng bị trùng lặp tên đăng ký
        /// </summary>
        duplicateUserName=2,

        /// <summary>
        /// lỗi gặp khi độ dài mật khẩu nhỏ hơn yêu cầu
        /// </summary>
        minLength=3,

        /// <summary>
       /// Đăng nhập thất bại
        /// </summary>
        loginFailed=4,

        /// <summary>
        /// đăng ký thất bại
        /// </summary>
        regiterFailed=5,

        /// <summary>
        /// Mật khẩu k trùng khớp
        /// </summary>
        confirmPassNotMatch=6,

        /// <summary>
        /// thông tin đầu vào trống
        /// </summary>
        EmptyModel=7,

        /// <summary>
        /// validate lỗi
        /// </summary>
        validateFeiled=8,


    }
}
