export default{
    
    /**
     * Giới tính  
     * Author: HMHieu -Date(21-09-22)                                                                                      
     */
    gender:{
        Male:0,//quy ước cho giới tính nam
        Femail:1,//quy ước cho giới tính nữ
        Other:2//quy ước cho các giới tính khác
    },

     /**
     * Trạng thái làm việc
     * Author: HMHieu -Date(19-09-22)
     */
      statusEmployee:{
        Woking:1,//đang làm việc
        UnWoking:0,//ngừng làm việc  
    },
    /**
     * chế độ form khi tiến hành sử dụng
     * Author: HMHieu -Date(19-09-22)
     */
    formMode:{
        ADD:1,//form thêm dữ liệu
        Edit:2,//form sửa dữ liệu
        Duplicate:3//form nhân đôi dữ liệu
    },
      /**
     * chế độ form khi tiến hành cất
     * Author: HMHieu -Date(19-09-22)
     */
    saveFormMode:{
        Save:1,//lưu
        SaveAndAdd:2//Lưu và thêm mới
    },
    /**
     * Các mã lỗi trả về từ backend
     * Author: HMHieu -Date(19-09-22)
     */
    errorBackEnd:{
        Ser:500,//lỗi server
        User:400,//lỗi người dùng
        NotFound:404,//lỗi k đúng đường dẫn
        Success:200,//thành công

        DuplicateCode:2,// trùng mã nhân viên
        InsertFailCode:4||3,// Dữ liệu đầu vào bị lỗi
        exception:1,// exception
        notFileImport:6,

        //error status login
        Login:401,//lỗi đăng nhập authen
        Forbidden:403,// lỗi phân quyền author
        DuplicateUsername:2,
        errorMinlength:3,
        loginFailed:4,
        regiterFailed:5,
        confirmPassNotMatch:6,
        EmptyModel:7,
        
    },
    errorFE:{
        UserActionError:10,
    },

    //Paging default
    page:{
        pageIndexDefault:1,//trng chọn default 
        pageSizeDefault:20, // size page default
    },

    marginTop:{
        marginTopEdit:35//margin top cột edit
    },

    //thời gian chờ
    timeOut:{
        loadData:500,//thời gian chờ load lại dữ liệu

        loadSearch:2000,// thời gian chờ sau khi thao tác xong sẽ search

        waitToast:1200,

        waitRadom:90000
    },
    /**
     * KeyCode
     * Author: HMHieu -Date(19-09-22)
     */
     KeyCode:{
        Enter:13,
        ESC:27,
        S:83,
        KeyDown:40,
        KeyUp:38

    },
     /**
     * Các mã xóa 
     * Author: HMHieu -Date(19-09-22)
     */
    popUpMode:{
        DeleteOne:1,//xóa một bản ghi
        DeleteMultiple:2,//xóa nhiều
        CloseImport:3,//Đóng form import
        ErrorAccount:4// Đóng form và đăng xuất khỏi hệ thống
    },

    popUpModeOneBtn:{
        warning:1,
        logout:2
    }
}