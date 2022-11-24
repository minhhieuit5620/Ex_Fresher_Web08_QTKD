let Resource = {};

/**
 * URL chung
 */
Resource.URL={
  auth:'http://localhost:5258/api/v1/',
  
}
//mã trùn
Resource.Duplicate={
    DuplicateBefore:"Mã nhân viên <",
    DuplicateAfter:">đã tồn tại trong hệ thống, vui lòng kiểm tra lại."
}
//xóa một bản ghi
Resource.DeleteOne={
  DeleteOneBefore:"Bạn có thực sự muốn xóa nhân viên <",
  DeleteOneAfter:"> không?"
  // `Bạn có thực sự muốn xóa nhân viên <${code}> không?`
}

//Tên file export
Resource.NameFileDownLoad={
  NameFile:"Danh_sach_nhan_vien.xlsx"
}


/**
 * Toast 
 */
Resource.ToastMessage = {
    success: 'Thành công',
    successDeleteMultiple: ' nhân viên đã xóa thành công',
    error: 'Thất bại',
    notComplete:'Chức năng đang trong quá trình thi công'
}

//session trong paging
Resource.Session={
  recordPage:"recordPage",
  textPage:"textPage",
  index:"index",

}

/**
 * số bản ghi hiển thị trên 1 trang
 */
Resource. textNumPages = [
  {
    num: 10,
    textData: '10 bản ghi trên một trang'
  },
  {
    num: 20,
    textData: '20 bản ghi trên một trang'
  },
  {
    num: 30,
    textData: '30 bản ghi trên một trang'
  },
  {
    num: 50,
    textData: '50 bản ghi trên một trang'
  },
  {
    num: 100,
    textData: '100 bản ghi trên một trang'
  },
],

// popUp Error validate
Resource.popupError = {
  EmptyName: {
    name: 'EmptyName',
    content: "Tên không được để trống."
  },
  EmptyCode: {
    name: 'EmptyCode',
    content: "Mã nhân viên không được để trống."
  },
  EmptyDepartment: {
    name: 'EmptyDepartment',
    content: "Đơn vị không được để trống."
  },
  ErrorMaxCode:{
    name:'ErrorMaxCode',
    content:"Mã nhân viên không được lớn hơn 20 ký tự."
  },
  EmptyFile:{
    name:'EmptyFile',
    content:"Bạn chưa chọn file, vui lòng chọn file để tiếp tục."
  },
  ErrorFile:{
    name:'ErrorFile',
    content:"File không đúng định dạng, vui lòng chọn lại để tiếp tục."
  },
  ErrorNextStep3:{
    name:'ErrorNextStep3',
    content:"Không có bản ghi nào hợp lệ, vui lòng kiểm tra file của bạn và tiếp tục."
  },
  UserActionError:{
    name:'UserActionError',
    content:"Hệ thống bị gặp lỗi, vui lòng liên hệ MISA."
  }
}

//popUp Error backend
Resource.popupErrorBackend = {
  Server: {
    name: 'ServerError',
    content: "Có lỗi xảy ra ở phía máy chủ"
  },
  User: {
    name: 'User',
    content: "Dữ liệu nhập vào của bạn không đúng, vui lòng kiểm tra lại"
  },
  NotFound: {
    name: 'NotFound',
    content: "Không tìm thấy"
  },
  Deleted: {
    name: 'Deleted',
    content: "Dữ liệu của bạn đã có sự thay đổi hoặc không tồn tại, vui lòng lấy lại dữ liệu"
  },
  NotExitsFile:{
    name:'NotExits',
    content:"File dữ liệu của bạn bị trống, vui lòng chọn file để tiếp tục."
  }
}

//PopUp cảnh báo
Resource.PopupWarning={
  CloseImport:"Bạn có thực sự muốn kết thúc quá trình nhập khẩu không?",
  CloseDeleteMul:"Bạn có thực sự muốn xóa những nhân viên đã chọn không?",
  EmptyCode:"Mã nhân viên không tồn tại trong hệ thống vui lòng kiểm tra lại.",
  ErrorEmailFormat:"Email chưa đúng định dạng",
  ErrorMaxDateOfBirth:"Ngày sinh không được lớn hơn ngày hiện tại",
  ErrorMaxDateIdentity:"Ngày cấp không được lớn hơn ngày hiện tại",
  MinDateIdentity:"Ngày cấp không được nhỏ hơn ngày sinh ",
  ErrorAccount:"Tài khoản của bạn đã bị thay đổi, hệ thống sẽ tự động đăng xuất. Vui lòng đăng nhập lại để tiếp tục."

}

Resource.emit={
  errorBackEnd:"errorBackEnd",
  loadData:"loadData",
  openToast:"openToast",
  closeDialogAdd:"closeDialog",
  reloadForm:"reLoadForm",

  //import
  errorTypeFile:"errorTypeFile",
  noRecordValid:"noRecordValid",
  showPopupClose:"showPopupClose"
}

//Thông tin liên quan đến bảo mật
Resource.Manager = {
  //token
  token:{
    accessToken:"token",
    refreshToken:"refreshToken",
    rol:"role",
    username:"username"
  },
  // vai trò
  role:{
    admin:"Admin",
    user:"User",
    supperAdmin:"SupperAdmin"
  }
}

//method
Resource.method={
  POST:"POST",
  PUT:"PUT",
  GET:"GET",
  DELETE:"DELETE",
}


export default Resource;
