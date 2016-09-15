using System;
using JpAnnotator.Common.Bundling;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Common.Portable.PlainText;
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
            var resourceLocator = new ConsoleAppResourceLocator();
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
                new ContentsBreaker(new ChapterMarkersProvider(options, new ContentsDetector())),
                new EpubMaker(log, options, resourceLocator),
                new SentenceBreaker()
            );

            options.Print(Console.Out);

            _htmlToEpub.Convert().ContinueWith(_ =>
            {
                log.Debug("end");
            }).Wait();
        }
    }
}
