using System;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Common.Windows;
using JpAnnotator.Core;
using JpAnnotator.Core.Dic;
using JpAnnotator.Core.Make.Epub;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Logging;

namespace JpAnnotator
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var resourceLocator = new WindowsResourceLocator();
            var log = new LoggingConfig(resourceLocator).CreateRootLogWriter();
            log.Debug("start");

            var options = new Options(args);

            var _htmlToEpub = new HtmlToEpubConverter(
                new Counter(log),
                log,
                options,
                new MecabParser(),
                new MecabReader(),
                new MecabBackend(),
                new XHtmlMaker(),
                new JmdicFastReader(
                    log,
                    resourceLocator,
                    new Jmdictionary()
                ),
                new ContentsBreaker(options),
                new EpubMaker(log, options, resourceLocator)
            );

            options.Print(Console.Out);

            _htmlToEpub.Convert().ContinueWith(_ =>
            {
                log.Debug("end");
            }).Wait();
        }
    }
}
