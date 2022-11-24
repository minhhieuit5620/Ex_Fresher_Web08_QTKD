using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Microsoft.Extensions.DependencyInjection;
using Misa.AMIS.DL.UOW;

namespace Misa.AMIS.DL.MySQL
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Inject DB connection và UnitOfWork
        /// </summary>
        public static void AddDapperMySql(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<DbConnection>(provider =>
            {
                return new MySqlConnection(connectionString);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
