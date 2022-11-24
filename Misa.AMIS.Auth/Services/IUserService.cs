

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Misa.AMIS.Auth.Common;
using Misa.AMIS.Auth.Common.Entities;
using Misa.AMIS.Auth.Common.Entities.DTO;
using Misa.AMIS.Auth.Model.DTO;
using Misa.AMIS.Auth.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Misa.AMIS.Auth.Services
{
    #region Interface
    public interface IUserService //interface
    {
        Task<ServiceResponse> RegisterUser(RegisterModel registerModel);
        Task<TokenServiceResponse> Login(LoginModel loginModel);
        Task<TokenServiceResponse> RefreshToken(TokenModel tokenModel);
        Task<ServiceResponse> ReVoke(string username);
        Task<ServiceResponse> RevokeAll();
    } 
    #endregion

    public class UserService : IUserService //impliment
    {
        #region Feild
        private UserManager<ApplicationUserFull> _userManager;

        private RoleManager<IdentityRole> _roleManager;

        private IConfiguration _configuration;
        #endregion

        #region Contructor

        public UserService(
            UserManager<ApplicationUserFull> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        #endregion


        /// <summary>
        /// Đăng ký thông tin người dùng mới
        /// </summary>
        /// <param name="registerModel">thông tin người dùng</param>
        /// <returns>true + message nếu thành công/ false+ message nếu thất bại</returns>
        public async Task<ServiceResponse> RegisterUser(RegisterModel registerModel)
        {
            //Nếu tham số null
            if(registerModel == null)
            {
                return new ServiceResponse {
                    Success = false,
                    Data = new ErrorResult
                        (
                        ErrorCode.EmptyModel,
                        Resource.Empty
                        )                  
                };
            }

            //Nếu pass không trùng với Confirm 
            if(registerModel.PassWord != registerModel.ConfirmPassword)
            {
                return new ServiceResponse {
                    Success = false,
                    Data = new ErrorResult
                        (
                        ErrorCode.confirmPassNotMatch,
                        Resource.ErrorConfirmPass
                        )                  
                };
            }
            //kierm tra đã có tên người dùng trong hệ thống chưa
            var checkUserName = await _userManager.FindByNameAsync(registerModel.UserName);

            if(checkUserName == null)//nếu tên người dùng chưa tồn tại
            {
                //thông tin người dùng
                var identityUser = new ApplicationUserFull
                {
                    UserName = registerModel.UserName
                    //Email = registerModel.UserName

                };
                //Thực hiện thêm mới                     
                var result = await _userManager.CreateAsync(identityUser, registerModel.PassWord);

                //Thành công 
                if (result.Succeeded)
                {
                    //thêm role cho người dùng
                    await _userManager.AddToRoleAsync(identityUser, Role.User.ToString());

                    // trả về dữ liệu cho client
                    return new ServiceResponse
                    {
                        Success = true,
                        Data = Resource.RegisterSuccess
                    };
                }
                else
                {// thất bại
                    return new ServiceResponse
                    {
                        Success = false,

                        Data = new ErrorResult
                        (
                        ErrorCode.regiterFailed,
                         Resource.RegisterFailed
                        )


                       
                        //Resource.RegisterFailed,

                    };
                }
            }
            else
            {
                //nếu tên người dùng đã tồn tại
                return new ServiceResponse
                {
                    Success = false,

                    Data = new ErrorResult
                    (
                        ErrorCode.duplicateUserName,
                        Resource.UsernameExitsed
                    )                  
                  

                };
            }
          
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginModel">Thông tin đăng nhập</param>
        /// <returns>true + message nếu thành công/ false+ message nếu thất bại</returns>
        public async Task<TokenServiceResponse> Login(LoginModel loginModel)
        {
            //lấy thông tin người dùng theo username
            var user= await _userManager.FindByNameAsync(loginModel.UserName);

            
            if (user == null)
            {
                return new TokenServiceResponse
                {
                    Success = false,
                    Data = Resource.LoginFailed
                };
               
            }
            else
            {
                //lấy vai trò theo người  dùng 
                var roles = await _userManager.GetRolesAsync(user);

                //kiểm tra password 
                var pass = await _userManager.CheckPasswordAsync(user, loginModel.PassWord);

                //Nếu user bằng null hoặc kiểm tra mã thất bại
                if (user == null || pass == false || roles.Count == 0 || roles == null)
                {
                    return new TokenServiceResponse
                    {
                        Success = false,
                        Data = Resource.LoginFailed
                    };
                }
                else
                {
                    // thêm thông tin payload token
                    var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//ID của JWT

                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),// thời điểm phát hành token tính theo UNIX time

                    new Claim(ClaimTypes.Name, user.UserName),//Tên đăng nhập
                                                                //
                    new Claim(ClaimTypes.NameIdentifier, user.Id),//Id người dùng
                };

                    // thêm role
                    foreach (var userRole in roles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    //gọi hàm tạo token
                    var token = CreateToken(authClaims);

                    //gọi hàm ren mới một refresh token
                    var refreshToken = GenerateRefreshToken();

                    _ = int.TryParse(_configuration[Resource.RefreshTokenValidityInDays], out int refreshTokenValidityInDays);

                    // convert sang chuỗi
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    //update vào DB
                    await _userManager.UpdateAsync(user);

                    //Trả về dữ liệu cho client
                    return new TokenServiceResponse
                    {
                        Data = new
                        {
                            AccessToken = tokenString,
                            RefreshToken = refreshToken,
                            Expiration = token.ValidTo,
                            Rol = roles,
                            UserName=user.UserName,
                        },
                        Success = true,
                    };

                }
            }
                           

         
          
        }

        /// <summary>
        /// Làm mới token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns>Trả về token và refresh token để auth</returns>
        public async Task<TokenServiceResponse> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return new TokenServiceResponse
                {
                    Success=false,
                    Data=Resource.TokenModelNull,
                };
            }
            //gán Token mới
            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;
            
            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return new TokenServiceResponse
                {
                    Success = false,
                    Data = Resource.InvalidTokenOrRefreshToken,
                };
                //return BadRequest("Invalid access token or refresh token");
            }

            //get username
            string username = principal.Identity.Name;

            //tìm kiếm username trong DB
            var user = await _userManager.FindByNameAsync(username);

            //nếu không tồn tại user hoặc không hợp lệ
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new TokenServiceResponse
                {
                    Success = false,
                    Data = Resource.InvalidTokenOrRefreshToken,
                };
                //return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new TokenServiceResponse
            {
                Data = new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    refreshToken = newRefreshToken
                },
                Success=true
               
            };
        }

        
        /// <summary>
        /// Xóa bỏ refresh token
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ServiceResponse> ReVoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)//nếu k tồn tại
            {
                return new ServiceResponse
                {
                    Success = false,
                };
            }
            else
            {//nếu tồn tại
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);

                return new ServiceResponse
                {
                    Success = true,

                    Data = Resource.RevokeSuccess,
                };
            }   
        }

        /// <summary>
        /// Xóa bỏ tất cả các refresh token
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse> RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
            return new ServiceResponse
            {
                Success=true,
            };

        }

        /// <summary>
        /// Sinh token
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns></returns>
        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Resource.JwtKey]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);// kiểu mã hóa

            var token = new JwtSecurityToken(
                issuer: _configuration[Resource.JwtIssuer],
                audience: _configuration[Resource.JwtAudience],
                claims: authClaims,
                expires: DateTime.UtcNow.AddSeconds(20),// xét thời gian tồn tại 
                signingCredentials: signIn);//chữ ký
            return token;
        }

        /// <summary>
        /// Sinh chuỗi refreshtoken
        /// </summary>
        /// <returns>chuỗi refreshtoken</returns>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// tạo mới token từ  cái cũ
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
         
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Resource.JwtKey])),
                ValidateLifetime = false//k check thời gian hết hạn 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            //validate token
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || // check xem co phải kiểu token khong
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, //check hiểu mã hóa
                StringComparison.InvariantCultureIgnoreCase)//không phân biệt hoa thường
                )
            {
                throw new SecurityTokenException(Resource.InvalidTokenOrRefreshToken);
            }    
               

            return principal;
        }
    }
}
