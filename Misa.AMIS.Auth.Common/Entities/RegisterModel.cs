using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Auth.Common.Entities
{
    public class RegisterModel
    {
        [Required]
        
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20),MinLength(4)]
        public string PassWord { get; set; }

        [Required,StringLength(20),MinLength(4)]
        public string ConfirmPassword { get; set; }

        //[Required]
        //public string Role { get; set; }
    }
}
