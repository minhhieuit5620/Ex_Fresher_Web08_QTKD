using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misa.AMIS.DL.BaseDL;

namespace Misa.AMIS.DL.UOW
{
    public interface IUnitOfWork: IDisposable
    {       
        /// <summary>
        /// chuỗi kết nối
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// transaction
        /// </summary>
        DbTransaction Transaction { get; }

        /// <summary>
        /// bắt đầu UOW
        /// </summary>
        void Begin();

        /// <summary>
        /// commit nếu thành công
        /// </summary>
        void Commit();

        /// <summary>
        /// quay lại để bảo toàn dữ liệu
        /// </summary>
        void Rollback();
  
    }
}
