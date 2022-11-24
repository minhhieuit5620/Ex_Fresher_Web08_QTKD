using Misa.AMIS.DL.UOW;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.AMIS.DL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Feild

        public DbConnection Connection { get; set; }

        public DbTransaction Transaction { get; set; }
        #endregion

        #region Contructor
        public UnitOfWork(DbConnection connection)
        {
            Connection = connection;
        }

        #endregion
        /// <summary>
        /// bắt đầu transaction 
        /// </summary>
        public void Begin()
        {
            Transaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// thực hiện nếu không lỗi
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();
        }
     
        /// <summary>
        /// giải phóng bộ nhớ
        /// </summary>
        public void Dispose()
        {
            if (Transaction != null)
                Transaction.Dispose();

            Transaction = null;
        }

        /// <summary>
        /// quay lại trước khi thực hiện
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
        }
      
    }
}
