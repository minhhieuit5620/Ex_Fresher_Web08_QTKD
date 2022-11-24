using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.Auth.Common.Entities
{
    public class TokenModel
    {
        //token hiện tại
        public string? AccessToken { get; set; }

        /// <summary>
        /// token dùng để refresh
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
