using Dapper;
using Misa.AMIS.Common.Attributes;
using Misa.AMIS.Common.Entities;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.DL.BaseDL
{
    public class BaseDL<T> : IBaseDL<T>
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
            if (recordDelete > 0)
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
        public PagingData<T> GetRecordsByFilter(string? search, string? sort, int pageIndex, int pageSize)
        {
            var procName = $"Proc_{typeof(T).Name.ToLower()}_GetDataFilterPaging";

            var parameters = new DynamicParameters();

            parameters.Add("v_Offset", (pageIndex - 1) * pageSize);

            parameters.Add("v_Limit", pageSize);

            parameters.Add("v_Sort", sort);

            string vWhere = "";

            var recordCode = $"{typeof(T).Name}Code";

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
        public virtual string Insert(T record)
        {
            var newID = Guid.NewGuid();

            var properties = typeof(T).GetProperties();

            var parameters = new DynamicParameters();           

                foreach (var prop in properties)
                {
                    string propName = prop.Name;

                    object propValue;

                    var primaryKeyAttribute = (Primarykey?)Attribute.GetCustomAttribute(prop, typeof(Primarykey));

                    if (primaryKeyAttribute != null)
                    {
                        propValue = newID;
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
                    if (mySQLConnection.State != System.Data.ConnectionState.Open)
                    {
                        mySQLConnection.Open();
                    }
                    //mySQLConnection.Open();
                    using (var tran = mySQLConnection.BeginTransaction())
                    {
                        string nameProc = $"Proc_{typeof(T).Name.ToLower()}_InsertOne";

                        numberOfAffecttedRows = mySQLConnection.Execute(nameProc, parameters, transaction: tran, commandType: System.Data.CommandType.StoredProcedure);

                        if (numberOfAffecttedRows > 0)
                        {
                            tran.Commit();
                            return newID.ToString();
                        }
                        else
                        {
                            tran.Rollback();
                            return Guid.Empty.ToString();
                        }
                    }
                }

            //}
            //else
            //{
            //    return null;
            //}


        }

        /// <summary>
        ///lấy một bản ghi theo mã
        /// </summary>      
        /// <param name="ID"></param>
        /// <returns>dữ liệu bản ghi tương ứng với mã đã nhập</returns>
        /// Created by: HMHieu(29/09/2022)
        public T GetRecordByID(Guid ID)
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
        public string Update(Guid ID, T record)
        {
            var properties = typeof(T).GetProperties();

            var parameters = new DynamicParameters();                     

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
                    if (mySQLConnection.State != System.Data.ConnectionState.Open)
                    {
                        mySQLConnection.Open();
                    }
                    //mySQLConnection.Open();
                    using (var tran = mySQLConnection.BeginTransaction())
                    {
                        string nameProc = $"Proc_{typeof(T).Name.ToLower()}_UpdateByID";

                        numberOfAffecttedRows = mySQLConnection.Execute(nameProc, parameters, transaction: tran, commandType: System.Data.CommandType.StoredProcedure);

                        if (numberOfAffecttedRows > 0)
                        {
                            tran.Commit();
                            return ID.ToString();
                        }
                        else
                        {
                            tran.Rollback();
                            return Guid.Empty.ToString();
                        }
                    }
                }     
        }

        /// <summary>
        /// Kiểm tra mã trùng
        /// CreatedBy: HMHieu(9/10/2022)
        /// </summary>
        /// <param name="record">bản ghi nhân viên</param>
        /// <returns>Mã nhân viên nếu bị trùng</returns>
        //public Guid checkDuplicateEmployeeCode(T record)
        //{

        //    //khai báo store proceduce
        //    string storedProceduceName = $"Proc_{typeof(T).Name.ToLower()}_CheckDuplicate";        

        //    var parameters = new DynamicParameters();

        //    var props = typeof(T).GetProperties();
        //    foreach (var prop in props)
        //    {
        //        string propName = prop.Name;

        //        string nameCheckCode = "EmployeeCode";
              
        //        if (propName == nameCheckCode)
        //        {
        //            var propValue = prop.GetValue(record, null);
        //            parameters.Add("v_EmployeeCode", propValue);
        //            break;
        //        }
        //        var isPrimarykey = (Primarykey?)Attribute.GetCustomAttribute(prop, typeof(Primarykey));
        //        if (isPrimarykey != null)
        //        {
        //            var propValue = prop.GetValue(record, null);

        //            parameters.Add("v_EmployeeID", propValue);
        //        }
        //        else parameters.Add("v_EmployeeID", Guid.Empty);

        //    }

        //    //kết nối đến db
        //    using (MySqlConnection connect = new MySqlConnection(DataContext.MySqlConnectionString))
        //    {
        //        //thực hiện câu lệnh 
        //        var idEmp = connect.QueryFirstOrDefault<Guid>(storedProceduceName, parameters, commandType: System.Data.CommandType.StoredProcedure);

        //        // Trả về dữ liệu cho client
        //        return idEmp;
        //    }
        //}
    }
}