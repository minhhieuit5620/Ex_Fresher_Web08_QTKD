using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Misa.AMIS.Auth.Common;
using Misa.AMIS.Auth.Common.Entities;
using Misa.AMIS.Auth.Services;
using Misa.AMIS.Auth.Common.HandleErrorAuth;
using Misa.AMIS.Auth.Common.Entities.DTO;

namespace Misa.AMIS.Auth.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Feild

        private IUserService _userService;

        #endregion

        #region Constructor
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="registerModel">thông tin người dùng đăng ký</param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                if (ModelState.IsValid)//kiểm tra dữ liệu truyền vào
                {//nếu hợp lệ

                    //gọi đến service
                    var result = await _userService.RegisterUser(registerModel);

                    if (result.Success)//thành công
                    {
                        return StatusCode(StatusCodes.Status201Created, result);
                    }
                    else//thất bại
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, result);
                    }
                }
                else//dữ liệu đầu ào không hợp lệ
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.validateFeiled));
                }
            }
            catch (Exception ex)
            {
                //Lưu lỗi vào nLog 
                HandleErrorAuth.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.exception));

            }
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginModel">Thông tin tài khoản mật khẩu</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {

                if (ModelState.IsValid)// kiểm tra dữ liệu truyền vào
                {//nếu hợp lệ
                    var result = await _userService.Login(loginModel);
                    if (result.Success)//thành công
                    {
                        return StatusCode(StatusCodes.Status200OK, result);
                    }
                    else//thất bại
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, result);
                    }
                }
                else//dữ liệu truyền vào không hợp lệ
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.validateFeiled));

                }
            }
            catch (Exception ex)
            {

                //Lưu lỗi vào nLog 
                HandleErrorAuth.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.exception));

            }
        }

        /// <summary>
        /// Làm mới token
        /// </summary>
        /// <param name="tokenModel"> </param>
        /// <returns>Trả về token mới</returns>
        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {

            try
            {
                var refreshToken = await _userService.RefreshToken(tokenModel);
                if (refreshToken.Success)//thành công
                {
                    return StatusCode(StatusCodes.Status200OK, refreshToken);
                }
                else//thất bại
                {
                    return StatusCode(StatusCodes.Status403Forbidden, refreshToken);
                }
            }
            catch (Exception ex)
            {
                //Lưu lỗi vào nLog 
                HandleErrorAuth.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.exception));

            }

        }

        /// <summary>
        /// xóa refresh token trong DB để phục vụ cho lần sử dụng sau
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            try
            {
                var revoke = await _userService.ReVoke(username);
                if (revoke.Success)//thành công
                {
                    return StatusCode(StatusCodes.Status200OK, revoke);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, revoke);
                }
            }
            catch (Exception ex)//thất bại
            {

                //Lưu lỗi vào nLog 
                HandleErrorAuth.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.exception));

            }
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            try
            {
                var revoke = await _userService.RevokeAll();
                if (revoke.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, revoke);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, revoke);
                }
            }
            catch (Exception ex)
            {
                //Lưu lỗi vào nLog 
                HandleErrorAuth.HandleNlog(ex);
                //trả về trạng thái lỗi cho client và mã lỗi cho hàm handle
                return StatusCode(StatusCodes.Status500InternalServerError, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.exception));

            }
        }
        #endregion

    }
}
