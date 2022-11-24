using Microsoft.EntityFrameworkCore;

namespace Misa.AMIS.Auth.Model
{
    public class DataContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer();
        //}
        public static string AuthSqlConnectionString;
    }
}
