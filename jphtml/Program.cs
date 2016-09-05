using System.IO;
using JpAnnotator.Common.Windows;
using JpAnnotator.Core;
using JpAnnotator.Core.Dic;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Logging;

namespace JpAnnotator
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var log = new LoggingConfig(new WindowsResourceLocator()).CreateRootLogWriter();
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
                    options,
                    Path.Combine(FileSystemUtils.AppDir, "data", "dic", "JMdict_e"),
                    new Jmdictionary()
                ),
                new ContentsBreaker(
                    options.OutputDir,
                    options.ChapterMarkers
                )
            );

            options.Print();

            _htmlToEpub.Convert();
            log.Debug("end");
        }
    }
}
