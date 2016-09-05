using System.IO;
using System.Reflection;
using log4net.Config;
using jphtml.Logging;

namespace jphtml
{
    public static class LoggingConfig
    {
        static LoggingConfig()
        {
            XmlConfigurator.Configure(new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "log4net.config")));
        }

        public static ILogWriter CreateRootLogWriter() => new Log4netWriter("root");
    }
}

