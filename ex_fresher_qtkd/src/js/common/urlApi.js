import Resource from "./resource";

const loadDataURL =process.env.VUE_APP_URL;//local back end
const loadDataAuth=Resource.URL.auth;
export default{   

    /********************
    * API thực hiện nhập khâu nhiều bước
    * HMHieu 17-11-2022
    * 
    */
    importData:{
    //api check dữ liệu khi gửi file về BE
        checkImport:loadDataURL+"Employees/check-import",
        //api thêm dữ liệu vào DB
        import:loadDataURL+"Employees/import-steps",
        //api xuất khẩu dữ liệu đã nhập khẩu thành công
        exportImportSuccess:loadDataURL+"Employees/export-import",
    },

    /********************
    * API  nhập khẩu dữ liệu một bước
    * HMHieu 17-11-2022
    * 
    */
    importDefault:{
        import:loadDataURL+"Employees/import-default"
    },

    /********************
    * Api thêm, nhân bản, xóa nhiều, lấy mã nhân viên mới
    * HMHieu 17-11-2022
    * 
    */
    methodCRUD:{
        insert:loadDataURL+"Employees",//thêm mới hoặc nhân bản
        // update:loadDataURL,
         getEmployeeById:loadDataURL+"Employees/#",
        // delete:loadDataURL,
        deleteMultiple:loadDataURL+'Employees/delete-multiple',//xóa nhiều
        getNewCode:loadDataURL+"Employees/new-code-employee",//lấy mã nhân viên mới
        getDepartment:loadDataURL+"Departments"
    },

    /********************
    * APi load dữ liệu khi search và phân trang
    * HMHieu 17-11-2022
    * 
    */
    apiGet(search,pageIndex,pageSize){
        var url=loadDataURL+`Employees/filter?search=${search}&pageIndex=${pageIndex}&pageSize=${pageSize}`
        return url
    },

    /********************
    * API thay đổi trạng thái làm việc của nhân viên
    * HMHieu 17-11-2022
    * 
    */
    apiChangeStatus(status,Id){
        var url=loadDataURL+`Employees/change-status?statusEmployee=${status} &EmployeeID=${Id}`;  
        return url
    },

    /********************
    * api xóa hoặc cập nhật nhân viên
    * HMHieu 17-11-2022
    * 
    */
    apiDeleteOrPut(Id){
        var url=loadDataURL+`Employees/${Id}`;  
        return url
    },

    /********************
    * API xuất khẩu excel
    * HMHieu 17-11-2022
    * 
    */
    apiExportExcel(searchKey){
        var url=loadDataURL+`Employees/export-excel?search=${searchKey}`;  
        return url
    } , 

    /*****************************************
     * Các đầu API Auth
     */

    apiAuth:{
        //đăng nhập
        login:loadDataAuth+"Auth/Login",

        //đăng ký
        register:loadDataAuth+'Auth/Register',

        //lấy tên vai trò
        getRole:loadDataAuth+"UsersManager/GetRole",

        //lấy tên người dùng
        getUser:loadDataAuth+"UsersManager/GetUser",

        //lấy lại token mới
        refreshToken:loadDataAuth+"Auth/Refresh-Token"
    },

    /********************
    * xóa thông tin vai trò
    * HMHieu 17-11-2022
    * 
    */
    apiDeleteRole(nameRole){
        var url=loadDataAuth+`UsersManager/DeleteRole?idRole=${nameRole}`;  
        return url
    }, 
    
    /********************
    *Thêm mới vai trò 
    * HMHieu 17-11-2022
    * 
    */
    apiAddRole(nameRole){
        var url=loadDataAuth+`UsersManager/AddRole?roleName=${nameRole}`;  
        return url
    },

    /********************
    * xóa người dùng
    * HMHieu 17-11-2022
    * 
    */
    apiDeleteUser(username){
        var url=loadDataAuth+`UsersManager/DeleteUser?userName=${username}`;  
        return url
    }

}