using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Misa.AMIS.Auth.Models
{
    public class AppliationDBContext:IdentityDbContext<ApplicationUserFull>
    {
        public AppliationDBContext(DbContextOptions<AppliationDBContext> options) : base(options)
        {

        }

        //sử dụng để ghi đè phương thứcvà sử dụng tham số builder kiểu Modelbuilder để cấu hình các lớp thực thể
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
