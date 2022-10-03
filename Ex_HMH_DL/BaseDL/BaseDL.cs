using Dapper;
using Ex_HMH_Common.Attributes;
using Ex_HMH_Common.Entities;
using Ex_QTKD_API.Entities;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_HMH_DL.BaseDL
{
    public class BaseDL<T> :IBaseDL<T>
    {
        /// <summary>
        /// lấy toàn bộ các bản ghi
        /// </summary>
        /// <returns> Tất cả các bản ghi</returns>
        /// CreatedBy Hứa Minh Hiếu(30/09/2022)
        public IEnumerable<T> GetAllRecords()
        {
            //department_GetAllDeparment
            string procName = $"Proc_{typeof(T).Name.ToLower()}_GetAll{typeof(T).Name}";

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                var listRecords = mySQLConnection.Query<T>(procName, commandType: System.Data.CommandType.StoredProcedure);
                
                return listRecords; 
            }
             
        }

        /// <summary>
        /// xóa một bản ghi 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="ID"></param>
        /// <returns>Id của bản ghi vừa xóa </returns>
        public Guid Delete(Guid ID)
        {          
            var procName = $"Proc_{typeof(T).Name.ToLower()}_DeleteByID";

            var parameters = new DynamicParameters();

            parameters.Add($"v_{typeof(T).Name}ID", ID);

            var recordDelete = 0;

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                recordDelete = mySQLConnection.Execute(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);
              
            }
            if(recordDelete > 0)
            {
                return ID;
            }
            else
            {
                return Guid.Empty; 
            }
        }

        /// <summary>
        /// Lấy danh sách bản ghi theo filter
        /// </summary>
        /// <param name="search">chuỗi cần tìm kiếm</param>
        /// <param name="sort">sắp xếp</param>
        /// <param name="pageIndex">số trang </param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <returns>Danh sách bản ghi thỏa mãn điều kiện filter</returns>
        /// Created by: HMHieu(28/09/2022)
        public PagingData<T> Filter(string? search, string? sort, int pageIndex, int pageSize)
        {
            var procName = $"Proc_{typeof(T).Name.ToLower()}_GetDataFilterPaging";

            var parameters = new DynamicParameters();

            parameters.Add("v_Offset", (pageIndex - 1) * pageSize);

            parameters.Add("v_Limit", pageSize);

            parameters.Add("v_Sort", sort);

            string vWhere = "";

            var recordCode=$"{typeof(T).Name}Code";

            var recordName = $"{typeof(T).Name}Name";

            if (search != null)
            {
                vWhere = $"{recordCode} LIKE \'%{search}%\' OR  {recordName}  LIKE \'%{search}%\'  ";
            }
            parameters.Add("v_Where", vWhere);

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                // Thực hiện gọi vào DB
                var results = mySQLConnection.QueryMultiple(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var listRecords = results.Read<T>();

                var totalCount = results.Read<int>().Single();


                if (results != null)
                {
                    return new PagingData<T>()
                    {
                        Data = listRecords.ToList(),

                        TotalCount = totalCount,

                        TotalPage = totalCount / pageSize + 1,
                    };
                }
                else
                {
                    return new PagingData<T>();
                }

            }
          
        }

        /// <summary>
        /// Thêm một bản ghi mới
        /// </summary>
        /// <param name="record">dữ liệu của bản ghi mới</param>
        /// <returns>ID của bản ghi</returns>
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
                string nameProc = $"Proc_{typeof(T).Name.ToLower()}_InsertOne";

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

        /// <summary>
        ///lấy một bản ghi theo mã
        /// </summary>      
        /// <param name="ID"></param>
        /// <returns>dữ liệu bản ghi tương ứng với mã đã nhập</returns>
        /// Created by: HMHieu(29/09/2022)
        public T RecordByID(Guid ID)
        {
            var procname = $"Proc_{typeof(T).Name.ToLower()}_GetByID";

            // chuẩn bị các tham số truyền vào proc

            var parameters = new DynamicParameters();

            parameters.Add($"v_{typeof(T).Name}ID", ID);

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //thưc hiện gọi vào DB 
                var record = mySQLConnection.QueryFirstOrDefault<T>(procname, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return record;
            }
        }

        /// <summary>
        /// sửa một bản ghi 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="ID"></param>
        /// <param name="record"></param>
        /// <returns>Return về Guid vừa sửa, Guid rỗng nếu thất bại </returns> 
        public Guid Update(Guid ID, T record)
        {
            var parameters = new DynamicParameters();

            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                string propName = prop.Name;

                object propValue;

                var primaryKeyAttribute = (Primarykey?)Attribute.GetCustomAttribute(prop, typeof(Primarykey));

                if (primaryKeyAttribute != null)
                {
                    propValue = ID;
                }
                else
                {
                    propValue = prop.GetValue(record, null);
                }
                parameters.Add("v_" + propName, propValue);
            }
            int numberOfAffecttedRows = 0;

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                string nameProc = $"Proc_{typeof(T).Name.ToLower()}_UpdateByID";
            
                numberOfAffecttedRows = mySQLConnection.Execute(nameProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

            }
            if (numberOfAffecttedRows > 0)
            {
                return ID;
            }
            else
            {
                return Guid.Empty;
            }
        }

      
    }
}
