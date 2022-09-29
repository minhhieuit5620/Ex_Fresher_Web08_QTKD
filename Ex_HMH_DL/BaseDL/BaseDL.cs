using Dapper;
using Ex_HMH_Common.Attributes;
using Ex_HMH_Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_HMH_DL.BaseDL
{
    public class BaseDL<T>
    {


        public Guid Insert(T record)
        {                    
            var newID = Guid.NewGuid();

            var parameters = new DynamicParameters();

            var properties=typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                string propName = prop.Name;

                object propValue;

                var primaryKeyAttribute = (Primarykey?)Attribute.GetCustomAttribute(prop, typeof(Primarykey));

                if(primaryKeyAttribute != null)
                {
                    propValue = newID;
                }
                else
                {
                    propValue = prop.GetValue(record, null);
                }
                parameters.Add("v_" + propName,propValue);
            }
            int numberOfAffecttedRows = 0;
            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                string nameProc = "Proc_employee_InsertOne";

                 numberOfAffecttedRows = mySQLConnection.Execute(nameProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

            }
            if (numberOfAffecttedRows > 0)
            {
                return newID;
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}
