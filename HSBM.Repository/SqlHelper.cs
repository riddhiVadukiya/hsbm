using BLToolkit.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Repository
{
    public class SqlHelper
    {
        public static string TablePrefix
        {
            get
            {
#if !SQL
                return "DBO.";
#else
                return "DSP.";
#endif
            }
        }

        public static void BeginTransaction(ref DbManager _DbManager)
        {
            try
            {
                _DbManager.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static void CommitTransaction(ref DbManager _DbManager)
        {
            try
            {
                _DbManager.CommitTransaction();
                _DbManager.Connection.Close();
                _DbManager.Connection.Dispose();
                _DbManager = null;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static void RollbackTransaction(ref DbManager _DbManager)
        {
            try
            {
                if (_DbManager != null)
                {
                    _DbManager.RollbackTransaction();
                    _DbManager.Connection.Close();
                    _DbManager.Connection.Dispose();
                    _DbManager = null;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static void CloseConnection(ref DbManager _DbManager)
        {
            try
            {
                if (_DbManager != null)
                {
                    _DbManager.Connection.Close();
                    _DbManager.Connection.Dispose();
                    _DbManager = null;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
