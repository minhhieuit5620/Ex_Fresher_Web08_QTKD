namespace Misa.AMIS.Common.Attributes
{
    /// <summary>
    /// Atribute dùng dể xác định 1 prop là khóa chính
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Primarykey : Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class IsNotNullOrEmptyAttribute : Attribute
    {
        #region MyRegion
        /// <summary>
        /// MSG lỗi trả về cho client
        /// </summary>
        public string ErrorMessage; 
        #endregion

        public IsNotNullOrEmptyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Atribute kiểm tra định dạng mail
    /// createdby: HMHieu(10/10/2022)
    /// </summary>
    public class IsNotEmailAttribute : Attribute
    {
        public string msg;

        public IsNotEmailAttribute(string msg)
        {
            this.msg = msg;
        }
    }

    /// <summary>
    /// Atribute kiểm tra ngày tháng
    /// createdby: HMHieu(10/10/2022)
    /// </summary>
    public class DateAttribute : Attribute
    {

        public string msg;

        public DateAttribute(string msg)
        {
            this.msg = msg;
        }
    }

    /// <summary>
    /// trùng mã
    /// </summary>
    public class DuplicateAttribute : Attribute
    {
    }
}
