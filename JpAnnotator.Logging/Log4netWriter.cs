using log4net;

namespace JpAnnotator.Logging
{
    public class Log4netWriter : ILogWriter
    {
        readonly ILog _log;

        public Log4netWriter(string name)
        {
            _log = LogManager.GetLogger(name);
        }

        bool ILogWriter.IsDebug => _log.IsDebugEnabled;
        bool ILogWriter.IsInfo => _log.IsInfoEnabled;
        bool ILogWriter.IsWarn => _log.IsWarnEnabled;
        bool ILogWriter.IsError => _log.IsErrorEnabled;

        void ILogWriter.Debug(string message) => _log.Debug(message);
        void ILogWriter.Info(string message) => _log.Info(message);
        void ILogWriter.Warn(string message) => _log.Warn(message);
        void ILogWriter.Error(string message) => _log.Error(message);
    }
}

