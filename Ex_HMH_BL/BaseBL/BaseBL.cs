
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Misa.AMIS.Common.Entities;
using Misa.AMIS.Common.Enums;
using Misa.AMIS.Common;
using Misa.AMIS.Common.Attributes;
using Misa.AMIS.DL;
using System.Text.RegularExpressions;

namespace Misa.AMIS.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Feild

        private IBaseDL<T> _baseDL;

       // protected List<string> ValidateErrorsMsg;
        protected List<string> validateFailed;
        // var validateFailed = new List<string>();

        #endregion

        #region Contractor

        public BaseBL(IBaseDL<T> baseDL) 
        {
            _baseDL = baseDL;
            //ValidateErrorsMsg = new List<string>();
            validateFailed = new List<string>();
        }

        #endregion

        #region Method
        /// <summary>
        /// lấy toàn bộ giá trị bản ghi của một bảng
        /// </summary>
        /// <param name="nameTable"></param>
        /// <returns>tất cả các giá trị của bảng được nhập</returns>   
        /// CreatedBy: HMHieu (29-09-2022)
        public IEnumerable<T> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
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
            return _baseDL.GetRecordsByFilter(search, sort, pageIndex, pageSize);
        }

        /// <summary>
        ///  lấy một bản ghi
        /// </summary>      
        /// <param name="ID"></param>
        /// <returns>dữ liệu bản ghi tương ứng với mã đã nhập</returns>
        /// Created by: HMHieu(18/09/2022)
        public T GetRecordByID(Guid ID)
        {
            return _baseDL.GetRecordByID(ID);
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>ID bản ghi vừa thêm mới nếu thành công, lỗi validate nếu thất bại</returns>
        public ServiceResponse Insert(T record)
        {
            var validateResults = ValidateRequestData(record);

            if (validateResults != null && validateResults.Success)
            {
                var newRecordID = _baseDL.Insert(record);
                if (newRecordID != Guid.Empty.ToString())
                {
                    return new ServiceResponse
                    {
                        Success = true,
                        Data = newRecordID
                    };                
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,                     

                        Data = new ErrorResult(

                        ErrorCode.InvalidInput,

                        Resource.DevMsg_InsertFailed,

                        userMsg: validateFailed[0],

                        Resource.Moreinfo_InsertFailed
                    )
                    };
                }
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                   
                    Data = new ErrorResult(

                        ErrorCode.InvalidInput,

                        Resource.DevMsg_InsertFailed,

                        userMsg: validateFailed[0],

                        Resource.Moreinfo_InsertFailed)
                };
            }
        }

        /// <summary>
        /// sửa một bản ghi 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="ID"></param>
        /// <param name="record"></param>
        /// <returns>Return về Guid vừa sửa, Guid rỗng nếu thất bại </returns>      
        public ServiceResponse Update(Guid ID, T record)
        {
            var validateResults = ValidateRequestData(record);

            if (validateResults != null && validateResults.Success)
            {
                var newRecordID = _baseDL.Update(ID,record);
                if (newRecordID != Guid.Empty.ToString())
                {
                    return new ServiceResponse
                    {
                        Success = true,
                        Data=newRecordID
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,

                        Data = new ErrorResult(

                        ErrorCode.InvalidInput,

                        Resource.DevMsg_InsertFailed,

                        userMsg: validateFailed[0],

                        Resource.Moreinfo_InsertFailed
                    )
                    };
                }
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,                 

                    Data = new ErrorResult(

                        ErrorCode.InvalidInput,

                        Resource.DevMsg_InsertFailed,

                        userMsg: validateFailed[0],

                        Resource.Moreinfo_InsertFailed)


                    
                };
            }
        }

        /// <summary>
        /// xóa một bản ghi 
        /// </summary>
        /// Created by: HMHieu(29/09/2022)
        /// <param name="ID"></param>
        /// <returns>Id của bản ghi vừa xóa </returns>
        public ServiceResponse Delete(Guid ID)
        {            
                var recordID = _baseDL.Delete(ID);

                if (recordID != Guid.Empty)
                {
                    return new ServiceResponse
                    {
                        Data = recordID,
                        Success = true
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,

                        Data = new ErrorResult(

                        ErrorCode.InvalidEmployeeCode,

                        Resource.DevMsg_DeleteFailed,

                        Resource.UserMsg_DeleteFailed,

                        Resource.Moreinfo_InsertFailed
                    )
                    };
                }           

            //return _baseDL.Delete(ID);
        }

        /// <summary>
        /// Hàm validate dữ liệu từ request
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ServiceResponse ValidateRequestData(T record)
        {
            //validate dữ liệu truyền vào
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)//lặp qua các prop để lấy giá trị
            {
                string propertyName = property.Name;//lấy giá trị propName

                var propertyValue = property.GetValue(record);//lấy value của prop

                var isNotNullOrEmptyAttr = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));

                //nếu có chứa attr và giá trị attr trống
                if (isNotNullOrEmptyAttr != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailed.Add(isNotNullOrEmptyAttr.ErrorMessage);
                }

                // định dạng mail
                //var isNotEmail = (IsNotEmailAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotEmailAttribute));
                //string regexEmail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                //Regex email = new Regex(regexEmail);
                //if (propertyValue != null && isNotEmail != null)
                //{
                //    if (propertyValue != "")
                //    {
                //        string emailValue = propertyValue.ToString();
                //        //var a = email.IsMatch(emailValue);
                //        if (emailValue != null && !email.IsMatch(emailValue))
                //            validateFailed.Add(isNotEmail.msg);
                //    }
                //}

                //// validate ngày tháng
                //var isDate = (DateAttribute?)Attribute.GetCustomAttribute(property, typeof(DateAttribute));
                //if (isDate != null && propertyValue != null)
                //{
                //    if ((DateTime)propertyValue > DateTime.Now)
                //    {
                //        validateFailed.Add(isDate.msg);
                //    }

                //}
            }
            ValidateObjectCustom(record);//validate các trường hợp riêng của từng bảng (Mã trùng, ngày tháng,email,..)
            //nếu tồn tại giá trị validate lỗi
            if (validateFailed.Count > 0)
            {
                return new ServiceResponse
                {
                    Success = false,

                    Data = validateFailed.FirstOrDefault().ToString()//lấy ra giá trị validate lỗi đầu tiên để gủi sang client
                    
                    
                };
            }
            else//thành công
            {
                return new ServiceResponse
                {
                    Success = true
                };
            }
            
        }

        /// <summary>
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <returns>List string các lỗi validate</returns>
        /// Thực hiện validate dữ liệu
        /// CreatedBy: HMHieu (20/10/2022)
        protected virtual List<string> ValidateObjectCustom(T entity)
        {
            return null;
        }

        #endregion
    }
}
