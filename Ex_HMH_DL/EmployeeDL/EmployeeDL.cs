using Dapper;
using Misa.AMIS.Common;
using Misa.AMIS.Common.Attributes;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Enums;
using Misa.AMIS.DL.BaseDL;
using Misa.AMIS.DL.DeparmentDL;
using Misa.AMIS.DL.UOW;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Misa.AMIS.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        private IUnitOfWork _unitOfWork;

        public EmployeeDL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lấy danh sách nhân viên theo filter
        /// </summary>
        /// <param name="search">chuỗi cần tìm kiếm</param>
        /// <param name="sort">sắp xếp</param>
        /// <param name="pageIndex">số trang </param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <returns>Danh sách nhân viên thỏa mãn điều kiện filter</returns>
        /// Created by: HMHieu(28/09/2022) 
        public PagingData<Employee> FilterEmployee(string? search, string? sort, int pageIndex, int pageSize)
        {
            var procName = "Proc_employee_GetDataFilterPaging";

            //Chuẩn bị tham số đầu vào cho Proc

            var parameters = new DynamicParameters();

            var offset = (pageIndex - 1) * pageSize;

            parameters.Add("v_Offset", offset);

            parameters.Add("v_Limit", pageSize);

            parameters.Add("v_Sort", sort);

            string vWhere = "";

            if (search != null)
            {
                vWhere = search;
                //$" EmployeeCode LIKE \'%{search}%\' OR EmployeeName LIKE \'%{search}%\' OR Mobile LIKE '%{search}%' OR LandlinePhone LIKE '%{search}%'  ";
            }
           
            parameters.Add("v_Where", vWhere);

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                // Thực hiện gọi vào DB
                var results = mySQLConnection.QueryMultiple(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var listEmployees = results.Read<Employee>();

                var totalCount = results.Read<int>().Single();


                return new PagingData<Employee>()
                {
                    Data = listEmployees.ToList(),

                    TotalCount = totalCount,

                    TotalPage = totalCount / pageSize + 1,
                };
            }
        }

        /// <summary>
        /// ghi đè lại phương thức Insert thực hiện ở Base
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public override string Insert(Employee employee)
        {
            //số bản ghi thay đổi
            var numberOfAffectedEmployee = 0;

            // Tạo GUID mới cho bản ghi cần thêm vào
            var newID = Guid.NewGuid();
            try
            {
                //lấy thông tin  Prop
                var propertiesEmployee = typeof(Employee).GetProperties();

                //lấy thông tin các params của employee
                var parametersEmployee = SetParamsEmployee(employee, newID);

                //mở kết nối
                _unitOfWork.Connection.Open();

                //bắt đầu tran
                _unitOfWork.Begin();

                //tìm kiếm thông tin phòng ban đã tồn tại hay chưa
                var department = _unitOfWork.Connection.QueryFirstOrDefault<Department>("SELECT * FROM Department WHERE DepartmentName=@name",
                    new { name = employee.DepartmentName }, transaction: _unitOfWork.Transaction);

                //nếu chưa tồn tại
                if (department == null)
                {
                    // Nếu đơn vị rỗng thì tạo đơn vị mới
                    department = new Department
                    {
                        DepartmentID = Guid.NewGuid(),
                        DepartmentName = employee.DepartmentName,
                    };
                    //department

                    //lấy thông tin các params của department
                    var parametersDepartment = SetParamDepartment(department);

                    //truyền tên phòng ban
                    parametersDepartment.Add("v_DepartmentName", department.DepartmentName);

                    //tạo mã phòng ban mới
                    string newCodeDepartment = newCodeDepartmentImport();

                    //gán mã phòng ban mới
                    parametersDepartment.Add("v_DepartmentCode", newCodeDepartment);

                    //lấy tên proc Insert Employee
                    string ProcInsertDepartment = $"Proc_{typeof(Department).Name.ToLower()}_InsertOne";

                    //thêm phòng ban mới
                    var numberOfAffectedDepartment = _unitOfWork.Connection.Execute(
                         ProcInsertDepartment,
                         parametersDepartment,
                         transaction: _unitOfWork.Transaction,
                         commandType: System.Data.CommandType.StoredProcedure);
                }

                // gán lại mã phòng ban(vì có thể phòng ban là mới)
                parametersEmployee.Add("v_DepartmentID", department.DepartmentID);

                //lấy tên proc Insert Employee
                string ProcInsertEmployee = $"Proc_{typeof(Employee).Name.ToLower()}_InsertOne";

                // thêm mới nhân viên
                numberOfAffectedEmployee = _unitOfWork.Connection.Execute(
                      ProcInsertEmployee,
                      parametersEmployee,
                      transaction: _unitOfWork.Transaction,
                      commandType: System.Data.CommandType.StoredProcedure
                      );

                // Không gặp lỗi thì commit
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                //nếu gặp lỗi
                _unitOfWork.Rollback();
            }
            finally
            {
                //giải phóng 
                _unitOfWork.Dispose();
            }

            // Trả về kết quả sau khi thực hiện truy vấn
            if (numberOfAffectedEmployee > 0)
            {
                // Nếu thành công thì trả về GUID bản ghi vừa thêm
                return newID.ToString();
            }
            else
            {
                // Nếu thất bại thì trả về GUID rỗng
                return Guid.Empty.ToString();
            }
        }

        /// <summary>
        /// Thay đổi trạng thái người dùng
        /// </summary>
        /// <returns>Guid nhân viên</returns>
        /// CreatedBy: Hứa Minh Hiếu
        public Guid ChangeStatus(Status statusEmployee, Guid EmployeeID)
        {
            var procName = "Proc_employee_ChangeStatus";

            var parameter = new DynamicParameters();

            parameter.Add("v_EmployeeID", EmployeeID);

            parameter.Add("v_Status", statusEmployee);

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                var numberOfAffecttedRows = mySQLConnection.Execute(procName, parameter, commandType: System.Data.CommandType.StoredProcedure);

                if (numberOfAffecttedRows > 0)
                {
                    return EmployeeID;
                }
                else  //Thát bại
                {
                    return Guid.Empty;
                }
            }
        }

        /// <summary>
        /// Tạo mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// CreatedBy: Hứa Minh Hiếu
        public string NewEmployeeCode()
        {
            var procName = "Proc_employee_GetMaxEmployeeCode";

            using (var mySQLConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                var maxCode = mySQLConnection.QueryFirstOrDefault<string>(procName, commandType: System.Data.CommandType.StoredProcedure);

                return maxCode.ToString();
            }
        }

        /// <summary>
        /// Kiểm tra mã trùng
        /// CreatedBy: HMHieu(9/10/2022)
        /// </summary>
        /// <param name="record">bản ghi nhân viên</param>
        /// <returns>Mã nhân viên nếu bị trùng</returns>
        public Guid checkDuplicateEmployeeCode(Employee record)
        {

            //khai báo store proceduce
            string storedProceduceName = $"Proc_{typeof(Employee).Name.ToLower()}_CheckDuplicate";

            var parameters = new DynamicParameters();

            var props = typeof(Employee).GetProperties();
            foreach (var prop in props)
            {
                string propName = prop.Name;

                string nameCheckCode = "EmployeeCode";

                if (propName == nameCheckCode)
                {
                    var propValue = prop.GetValue(record, null);

                    parameters.Add("v_EmployeeCode", propValue);
                    break;
                }
                var isPrimarykey = (Primarykey?)Attribute.GetCustomAttribute(prop, typeof(Primarykey));
                if (isPrimarykey != null)
                {
                    var propValue = prop.GetValue(record, null);

                    parameters.Add("v_EmployeeID", propValue);
                }
                else parameters.Add("v_EmployeeID", Guid.Empty);

            }

            //kết nối đến db
            using (MySqlConnection connect = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //thực hiện câu lệnh 
                var idEmp = connect.QueryFirstOrDefault<Guid>(storedProceduceName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Trả về dữ liệu cho client
                return idEmp;
            }
        }

        /// <summary>
        /// Xóa nhiều nhân viên cùng lúc
        /// </summary>
        /// <param name="listEmployeeID"></param>
        /// <returns>Số bản ghi đã xóa</returns>
        public int DeleteMultiple(List<Guid> listEmployeeID)
        {
            try
            {
                var procName = "Proc_employee_DeleteMultiple";

                string listEmployee = "";

                foreach (var employee in listEmployeeID)
                {
                    listEmployee += employee.ToString() + ",";
                }

                var a = listEmployee.Substring(0, listEmployee.Length - 1);

                var parameters = new DynamicParameters();

                parameters.Add("v_employeeID", a);
                //mở kết nối
                _unitOfWork.Connection.Open();

                //bắt đầu tran
                _unitOfWork.Begin();

                var numberOfAffecttedRows = _unitOfWork.Connection.Execute(procName, parameters, transaction: _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);

                if (numberOfAffecttedRows > 0)
                {
                    _unitOfWork.Commit();
                    return numberOfAffecttedRows;
                }
                else  //Thát bại
                {
                    _unitOfWork.Rollback();
                    return 0;
                }
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                return 0;
            }
            finally
            {
                //giải phóng 
                _unitOfWork.Dispose();
            }


        }

        /// <summary>
        /// kiểm tra dữ liệu đầu vào trong danh sách import đã có trong DB hay chưa
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>true nếu đã tồn tại/ false nếu chưa tồn tại</returns>
        public bool checkCodeImport(string employeeCode)
        {
            var check = false;
            var department = _unitOfWork.Connection.QueryFirstOrDefault<Employee>("SELECT * FROM Employee WHERE EmployeeCode=@code",
                      new { code = employeeCode });
            if (department != null)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            return check;
        }

        /// <summary>
        /// Nhập khẩu dữ liệu sử dụng proc(test)
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public async Task<int> ImportUsingProc(List<Employee> employees)
        {
            //mở kết nối
            _unitOfWork.Connection.Open();

            //bắt đầu tran
            _unitOfWork.Begin();
            try
            {

                Dictionary<string, object> listCheckDepartment = new Dictionary<string, object>();

                //danh sách phòng ban để xem đã tồn tại hay chưa
                var list = new List<string>();

                //khởi tạo danh sách dữ liệu phòng ban sẽ được thêm vào DB
                var listValuesDepartment = new List<string>();

                //lấy các phong ban đã có trong DB
                string sQuery = "SELECT DepartmentName from department";

                //lấy thông tin danh sách phòng ban đã có trong DB
                var departmentDB = _unitOfWork.Connection.Query<string>(sQuery, transaction: _unitOfWork.Transaction);

                //gán vào danh sách để check trong for
                list = departmentDB.ToList();

                //tạo mã phòng ban mới
                string Query = "SELECT MAX(DepartmentCode) from department";

                //lấy thông tin mã phòng ban lớn nhất
                var maxCodeDepartment = _unitOfWork.Connection.QueryFirstOrDefault<string>(Query, transaction: _unitOfWork.Transaction);

                //cắt chuỗi để lấy giá trị value của code
                var CodeDepartment = maxCodeDepartment.Substring(2);

                //chuyên về int
                int subNewCode = int.Parse(CodeDepartment) + 1;

                foreach (var item in employees)
                {
                    var departmentId = new Guid();
                    //kiểm tra đã lặp qua hay chưa
                    var checkNameDepartment = listCheckDepartment.ContainsKey(item.DepartmentName);

                    if (!checkNameDepartment)//nếu chưa tồn tại
                    {
                        listCheckDepartment.Add(item.DepartmentName, item);

                        //danh sách department chưa có
                        if (!list.Contains(item.DepartmentName))
                        {
                            list.Add(item.DepartmentName.ToString());
                            //tạo mã phòng ban mới

                            var newCode = "PB" + subNewCode++;

                            //danh sách dữ liệu phòng ban chưa tồn tại trong DB
                            listValuesDepartment.Add(string.Format("('{0}','{1}','{2}',{3},{4},{5},{6}),",
                                departmentId, newCode, item.DepartmentName, DateTime.Now, Resource.DefaultUser, DateTime.Now, Resource.DefaultUser));

                        }
                    }
                    else
                    {

                    }

                }
                var department = _unitOfWork.Connection.Query<Department>(" INSERT INTO department(DepartmentID, DepartmentCode, DepartmentName, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy) VALUES=@values",
                          new { values = listValuesDepartment }, transaction: _unitOfWork.Transaction);
                // Không gặp lỗi thì commit
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                //nếu gặp lỗi
                _unitOfWork.Rollback();
            }
            finally
            {
                //giải phóng 
                _unitOfWork.Dispose();
            }
            return 1;
        }

        /// <summary>
        /// Nhập khẩu dữ liệu
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public int ImportData(List<Employee> employees)
        {
            //số bản ghi thay đổi
            var numberOfAffectedEmployee = 0;

            try
            {
                //mở kết nối
                _unitOfWork.Connection.Open();

                //bắt đầu tran
                _unitOfWork.Begin();

                foreach (var employee in employees)
                {

                    // Tạo GUID mới cho bản ghi cần thêm vào
                    var newID = Guid.NewGuid();

                    //tìm kiếm thông tin phòng ban đã tồn tại hay chưa
                    var department = _unitOfWork.Connection.QueryFirstOrDefault<Department>("SELECT * FROM Department WHERE DepartmentName=@name",
                        new { name = employee.DepartmentName }, transaction: _unitOfWork.Transaction);

                    //nếu chưa tồn tại
                    if (department == null)
                    {
                        // Nếu đơn vị rỗng thì tạo đơn vị mới
                        department = new Department
                        {
                            DepartmentID = Guid.NewGuid(),
                            DepartmentName = employee.DepartmentName,
                        };
                        //department
                        //lấy các giá trị và gán giá trị vào parameters truyền xuống Proc
                        var parametersDepartment = SetParamDepartment(department);

                        //truyền tên phòng ban
                        parametersDepartment.Add("v_DepartmentName", department.DepartmentName);

                        // Tạo mã phòng ban mới
                        string newCodeDepartment = newCodeDepartmentImport();

                        //gán mã phòng ban mới
                        parametersDepartment.Add("v_DepartmentCode", newCodeDepartment);

                        //tên proc thêm mới phòng ban
                        string ProcInsertDepartment = $"Proc_{typeof(Department).Name.ToLower()}_InsertOne";

                        //thêm phòng ban mới
                        var numberOfAffectedDepartment = _unitOfWork.Connection.Execute(
                             ProcInsertDepartment,
                             parametersDepartment,
                             transaction: _unitOfWork.Transaction,
                             commandType: System.Data.CommandType.StoredProcedure);
                    }
                    //Lấy các thông tin gán vào parameters để truyền xuống Proc DB
                    var parametersEmployee = SetParamsEmployee(employee, newID);

                    // gán lại mã phòng ban(vì có thể phòng ban là mới)
                    parametersEmployee.Add("v_DepartmentID", department.DepartmentID);

                    //lấy tên proc Insert
                    string ProcInsertEmployee = $"Proc_{typeof(Employee).Name.ToLower()}_InsertOne";

                    // thêm mới nhân viên
                    numberOfAffectedEmployee += _unitOfWork.Connection.Execute(
                          ProcInsertEmployee,
                          parametersEmployee,
                          transaction: _unitOfWork.Transaction,
                          commandType: System.Data.CommandType.StoredProcedure
                          );
                }
                // Không gặp lỗi thì commit
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                //nếu gặp lỗi
                _unitOfWork.Rollback();
            }
            finally
            {
                //giải phóng 
                _unitOfWork.Dispose();
            }

            /// Nếu thành công thì trả về GUID bản ghi vừa thêm
            return numberOfAffectedEmployee;
        }

        /// <summary>
        /// Hàm set các parameters của phòng ban
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        private DynamicParameters SetParamDepartment(Department department)
        {
            //department
            var propertiesDepartment = typeof(Department).GetProperties();

            var parametersDepartment = new DynamicParameters();

            ///truyền các thông tin propName và prop value 
            foreach (var prop in propertiesDepartment)
            {
                string propName = prop.Name;

                object propValue;

                var primaryKeyAttribute = (Primarykey?)Attribute.GetCustomAttribute(prop, typeof(Primarykey));

                if (primaryKeyAttribute != null)
                {
                    propValue = department.DepartmentID;
                }
                else
                {
                    propValue = prop.GetValue(department, null);
                }
                parametersDepartment.Add("v_" + propName, propValue);
            }
            return parametersDepartment;
        }

        /// <summary>
        /// Hàm xét các parameters của nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="newID"></param>
        /// <returns>Các params hoàn chỉnh của bảng</returns>
        private DynamicParameters SetParamsEmployee(Employee employee, Guid newID)
        {
            //lấy thông tin  Prop
            var propertiesEmployee = typeof(Employee).GetProperties();

            //khai báo param
            var parametersEmployee = new DynamicParameters();

            //Lấy các thông tin prop của employee
            foreach (var prop in propertiesEmployee)
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
                    propValue = prop.GetValue(employee, null);
                }
                parametersEmployee.Add("v_" + propName, propValue);
            }
            return parametersEmployee;
        }

        /// <summary>
        /// Tạo mã phòng ban mới
        /// </summary>
        /// <returns>Mã phòng ban mới</returns>
        public string newCodeDepartmentImport()
        {
            //tạo mã phòng ban mới
            string sQuery = "SELECT MAX(DepartmentCode) from department";

            //lấy thông tin mã phòng ban lớn nhất
            var maxCodeDepartment = _unitOfWork.Connection.QueryFirstOrDefault<string>(sQuery, transaction: _unitOfWork.Transaction);

            //cắt chuỗi để lấy giá trị value của code
            var CodeDepartment = maxCodeDepartment.Substring(2);

            //chuyên về int
            int subNewCode = int.Parse(CodeDepartment) + 1;

            string newCodeDepartment = "PB" + subNewCode;
            return newCodeDepartment;

        }
    }
}
