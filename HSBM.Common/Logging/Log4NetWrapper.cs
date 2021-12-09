using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Common.Logging
{
    public class Log4NetWrapper : ILogger
    {
        private readonly ILog _logger;

        public Log4NetWrapper(Type type)
        {
            _logger = log4net.LogManager.GetLogger(type);
        }

        public Log4NetWrapper(string loggerName)
        {
            _logger = log4net.LogManager.GetLogger(loggerName);
        }

        public void Info(object message)
        {
            _logger.Info(message);
        }

        public void Info(object message, Exception exp)
        {
            _logger.Info(message, exp);
        }

        public bool IsInfoEnabled
        {
            get { return _logger.IsInfoEnabled; }
        }

        public void Debug(object message)
        {
            _logger.Debug(message);
            //OnLogRecieved("0", "Error", Convert.ToString(message), Status.Open, AlertType.ApplicationLog, "");
        }

        public void Debug(object message, Exception exp)
        {
            _logger.Debug(message, exp);
        }

        public bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }

        public void Warn(object message)
        {
            _logger.Warn(message);
        }

        public void Warn(object message, Exception exp)
        {
            _logger.Warn(message, exp);
        }

        public bool IsWarnEnabled
        {
            get { return _logger.IsWarnEnabled; }
        }

        public void Error(object message)
        {
            if (message is Exception)
            {
                Error("", (Exception)message);
            }
            else
            {
                _logger.Error(message);
            }
        }

        public void Error(object message, Exception exp)
        {
            _logger.Error(message, exp);
        }

        public bool IsErrorEnabled
        {
            get { return _logger.IsErrorEnabled; }
        }

        public void Fatal(object message)
        {
            if (message is Exception)
            {
                Fatal("", (Exception)message);
            }
            else
            {
                _logger.Fatal(message);
            }
        }

        public void Fatal(object message, Exception exp)
        {
            _logger.Fatal(message, exp);
        }

        public bool IsFatalEnabled
        {
            get { return _logger.IsFatalEnabled; }
        }

        public void SetUser(string userName)
        {
            log4net.GlobalContext.Properties["user"] = userName;
        }


    }
}
