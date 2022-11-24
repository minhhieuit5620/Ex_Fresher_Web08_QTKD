using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Auth.Common.Entities
{
    public  class LoginModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được phép trống")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "mật khẩu không được phép trống")]
        [StringLength(50), MinLength(4)]        
        public string PassWord { get; set; }
      
    }
}
