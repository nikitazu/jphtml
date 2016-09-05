using System;
using System.IO;
using log4net.Config;
using JpAnnotator.Common.Portable.Bundling;

namespace JpAnnotator.Logging
{
    public class LoggingConfig
    {
        public LoggingConfig(IResourceLocator resourceLocator)
        {
            var configFile = new FileInfo(Path.Combine(resourceLocator.ResourcesPath, "log4net.config"));
            if (configFile.Exists)
            {
                XmlConfigurator.Configure(configFile);
            }
            else
            {
                Console.Error.WriteLine($"Logging configuration file not found at: {configFile.FullName}");
            }
        }

        public ILogWriter CreateRootLogWriter() => new Log4netWriter("root");
    }
}
