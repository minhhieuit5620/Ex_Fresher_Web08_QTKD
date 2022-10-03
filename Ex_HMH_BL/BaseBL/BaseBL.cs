using Ex_HMH_Common;
using Ex_HMH_Common.Entities;
using Ex_HMH_Common.Enums;
using Ex_HMH_DL;
using Ex_HMH_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex_HMH_Common.Attributes;
using Ex_QTKD_API.Entities;

namespace Ex_HMH_BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Feild

        private IBaseDL<T> _baseDL;

        #endregion

        #region Contractor

        public BaseBL(IBaseDL<T> baseDL) 
        {
            _baseDL = baseDL;
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
        public PagingData<T> Filter(string? search, string? sort, int pageIndex, int pageSize)
        {
            return _baseDL.Filter(search, sort, pageIndex, pageSize);
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
                if (newRecordID != Guid.Empty)
                {
                    return new ServiceResponse
                    {
                        Data = newRecordID,
                        Success = true
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,

                        Data = new ErrorResult(

                        ErrorCode.InsertFailed,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

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

                    Data = validateResults.Data
                };
            }
        }

        /// <summary>
        ///  lấy một bản ghi
        /// </summary>      
        /// <param name="ID"></param>
        /// <returns>dữ liệu bản ghi tương ứng với mã đã nhập</returns>
        /// Created by: HMHieu(18/09/2022)
        public T RecordByID(Guid ID)
        {
            return _baseDL.RecordByID(ID);
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
                var recordID = _baseDL.Update(ID, record);

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

                        ErrorCode.InsertFailed,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

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

                    Data = validateResults.Data
                };
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
            return _baseDL.Delete(ID);
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

            var validateFailed = new List<string>();

            foreach (var property in properties)
            {
                string propertyName = property.Name;

                var propertyValue = property.GetValue(record);

                var isNotNullOrEmptyAttr = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));

                //nếu có chứa attr và giá trị attr trống
                if (isNotNullOrEmptyAttr != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailed.Add(isNotNullOrEmptyAttr.ErrorMessage);
                }
            }

            if (validateFailed.Count > 0)
            {
                return new ServiceResponse
                {
                    Success = false,

                    Data = new ErrorResult(

                        ErrorCode.InvalidInput,

                        Resource.DevMsg_InsertFailed,

                        Resource.DevMsg_validateFailed,

                        String.Join(" ,", validateFailed)
                    )
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Success = true
                };
            }
        }
        #endregion
    }
}
