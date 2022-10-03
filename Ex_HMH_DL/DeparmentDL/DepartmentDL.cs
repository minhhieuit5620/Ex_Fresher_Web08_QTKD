﻿using Dapper;
using Ex_HMH_Common.Entities;
using Ex_HMH_DL.BaseDL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_HMH_DL.DeparmentDL
{
    public class DepartmentDL:BaseDL<Department>,IDeparmentDL
    {
        /// <summary>
        /// lấy danh sách phòng ban 
        /// </summary>      
        /// <returns>dữ liệu danh sách phòng ban </returns>
        /// Created by: HMHieu(28/09/2022)
        //public IEnumerable<Department> AllDepartment()
        //{           
        //    var procname = "Proc_department_GetAllDeparment";
        //    using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
        //    {
        //        //thưc hiện gọi vào DB 
        //        var department = mySQLConnection.Query<Department>(procname, commandType: System.Data.CommandType.StoredProcedure);

        //        return department;
        //    }

        //}
    }
}
