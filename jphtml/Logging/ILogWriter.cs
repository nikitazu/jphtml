namespace jphtml.Logging
{
    public interface ILogWriter
    {
        bool IsDebug { get; }
        bool IsInfo { get; }
        bool IsWarn { get; }
        bool IsError { get; }

        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}

