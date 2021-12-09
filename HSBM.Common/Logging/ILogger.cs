using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Common.Logging
{
    public interface ILogger
    {
        void Info(object message);
        void Info(object message, Exception exp);
        bool IsInfoEnabled { get; }
        void Debug(object message);
        void Debug(object message, Exception exp);
        bool IsDebugEnabled { get; }
        void Warn(object message);
        void Warn(object message, Exception exp);
        bool IsWarnEnabled { get; }
        void Error(object message);
        void Error(object message, Exception exp);
        bool IsErrorEnabled { get; }
        void Fatal(object message);
        void Fatal(object message, Exception exp);
        bool IsFatalEnabled { get; }
    }
}
