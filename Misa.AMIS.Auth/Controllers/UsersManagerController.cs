using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.AMIS.Auth.Common;
using Misa.AMIS.Auth.Common.Entities;
using Misa.AMIS.Auth.Common.Entities.DTO;
using Misa.AMIS.Auth.Common.HandleErrorAuth;

using Misa.AMIS.Auth.Services;

namespace Misa.AMIS.Auth.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersManagerController : ControllerBase
    {
        #region Feild

        private IManagerUserService _managerUserService;

        #endregion

        #region Constructor
        public UsersManagerController(IManagerUserService managerUserService)
        {
            _managerUserService = managerUserService;
        }

        #endregion

        /// <summary>
        /// lấy tất cả thông tin vai trò
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRole")]
        public async Task<IActionResult> GetRole()
        {
            try
            {
                var rol = await _managerUserService.GetRoles();

                if (rol.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, rol);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, rol);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        /// <summary>
        /// Thêm mới vai trò
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                if (roleName == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.EmptyModel));
                }
                else
                {
                    var rol = await _managerUserService.AddRole(roleName);
                    if (rol.Success)
                    {
                        return StatusCode(StatusCodes.Status201Created, rol);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, rol);
                    }
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
        /// Xóa vai trò
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteRole")]
        public async Task<IActionResult> Delete(string idRole)
        {
            try
            {
                if (idRole == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.EmptyModel));
                }
                else
                {
                    var rol = await _managerUserService.DeleteRole (idRole);
                    if (rol.Success)
                    {
                        return StatusCode(StatusCodes.Status200OK, rol);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, rol);
                    }
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
        /// lấy tất cả thông tin người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUser")]                
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var user = await _managerUserService.GetUser();
                if (user.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, user);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, user);
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
        /// Thêm mới vai trò
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            try
            {
                if (userName == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleErrorAuth.HandleErrorAuthResult(ErrorCode.EmptyModel));                    
                }
                else
                {
                    var user = await _managerUserService.DeleteUser(userName);
                    if (user.Success)
                    {
                        return StatusCode(StatusCodes.Status200OK, user);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, user);
                    }
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

    }
}
