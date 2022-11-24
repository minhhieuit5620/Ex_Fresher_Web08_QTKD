using Misa.AMIS.Auth.Common.Entities.DTO;
using Misa.AMIS.Auth.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Misa.AMIS.Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Misa.AMIS.Auth.Services
{
    #region Interface
    public interface IManagerUserService //interface
    {
        Task<UserManagerResponse> GetRoles();
        Task<ServiceResponse> AddRole(string roleName);
        Task<UserManagerResponse> DeleteRole(string roleId);
        Task<UserManagerResponse> GetUser();
        Task<UserManagerResponse> GetUserRole(ApplicationUserFull user);
        Task<UserManagerResponse> DeleteUser(string userName);


    }
    #endregion
    #region Feild
    public class ManagerUserService : IManagerUserService
    {
        private UserManager<ApplicationUserFull> _userManager;

        private RoleManager<IdentityRole> _roleManager;

        private IConfiguration _configuration;
        #endregion

        #region Contructor

        public ManagerUserService(
            UserManager<ApplicationUserFull> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// lấy ra tất cả các thông tin của Role
        /// </summary>
        /// <returns></returns>
        public async Task<UserManagerResponse> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles != null)
            {
                return new UserManagerResponse
                {
                    Data = roles,

                    Success = true,

                };
            }
            else
            {
                return new UserManagerResponse { Success = false };
            }
        }

        /// <summary>
        /// Thêm mới một vai trò
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<ServiceResponse> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));

                return new ServiceResponse {
                    Success = true,
                    Data = roleName
                };
            }
            else
            {
                return new ServiceResponse { Success = false };
            }
        }

        /// <summary>
        /// xóa một vai trò
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns> 
        public async Task<UserManagerResponse> DeleteRole(string roleName)
        {
            if (roleName != null)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if(role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);                    

                    return new UserManagerResponse
                    {
                        Success = true,
                        Data = result
                    };
                }
                else
                {
                    return new UserManagerResponse { Success = false ,Data="Vai trò không tồn tại trong hệ thống"};
                }
                
            }
            else
            {
                return new UserManagerResponse { Success = false ,Data="Tên không được để trống"};
            }
        }

        /// <summary>
        /// Lấy ra tất cả các người dùng
        /// </summary>
        /// <returns></returns>
        public async Task<UserManagerResponse> GetUser()
        {
            List<UserRole> userRoles = new List<UserRole>();

            var users =await _userManager.Users.ToListAsync();

            foreach(var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);
                userRoles.Add(new UserRole
                {
                    UserName = item.UserName,
                    RoleName = roles.ToList()
                });
            }
            //new List<string>(await _userManager.GetRolesAsync(user));
            if (userRoles != null)
            {
                return new UserManagerResponse
                {
                    Data = userRoles,
                    Success = true,
                };
            }
            else
            {
                return new UserManagerResponse() { Success = false };
            }
        }

        /// <summary>
        /// lấy ra  các thông tin vai trò tương ứng với người dùng
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserManagerResponse> GetUserRole(ApplicationUserFull user)
        {
            var userRole = await _userManager.GetRolesAsync(user);

            if (userRole != null)
            {
                return new UserManagerResponse
                {
                    Data = userRole,
                    Success = true,
                };
            }
            else
            {
                   return new UserManagerResponse { Success=false };
            }
        } 

        /// <summary>
        /// Xóa thông tin người dùng thông qua Username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
         public async Task<UserManagerResponse> DeleteUser(string userName)
        {
            if (userName != null)
            {
                //tìm người dùng qua tên đăng nhập
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);

                    return new UserManagerResponse
                    {
                        Success = true,
                        Data = result
                    };
                }
                else
                {
                    return new UserManagerResponse { Success = false, Data = "Tài khoản không tồn tại trong hệ thống" };
                }

            }
            else
            {
                return new UserManagerResponse { Success = false, Data = "UserName không được để trống" };
            }
        }


        //public async Task<UserManagerResponse> UpdateRoleUser(string RolName)
        //{

        //    return new UserManagerResponse { Success = false, Data = "Tài khoản không tồn tại trong hệ thống" };
        //}


        #endregion

    }
}
