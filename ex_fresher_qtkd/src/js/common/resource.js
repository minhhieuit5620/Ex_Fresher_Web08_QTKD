let Resource = {};


Resource.ToastMessage = {
    success: 'Thành công',
    successDeleteMultiple: ' nhân viên đã xóa thành công',
    error: 'Thất bại',
    notComplete:'Chức năng đang trong quá trình thi công'
}


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


Resource.popupError = {
  EmptyName: {
    name: 'EmptyName',
    content: "Tên không được để trống"
  },
  EmptyCode: {
    name: 'EmptyCode',
    content: "Mã nhân viên không được để trống"
  },
  EmptyDepartment: {
    name: 'EmptyDepartment',
    content: "Đơn vị không được để trống"
  }
}
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
}
export default Resource;
