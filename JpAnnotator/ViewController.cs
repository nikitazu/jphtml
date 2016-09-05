using System;
using AppKit;
using Foundation;
using JpAnnotator.Logging;
using JpAnnotator.Common.Mac;
using JpAnnotator.Core;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Core.Dic;
using System.IO;

namespace JpAnnotator
{
    public partial class ViewController : NSViewController
    {
        ILogWriter _log;

        public ViewController(IntPtr handle) : base(handle)
        {
            _log = new LoggingConfig(new MacResourceLocator()).CreateRootLogWriter();
            _log.Debug("start");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            _log.Debug("view did load");
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void ConvertButtonClicked(NSObject sender)
        {
            _log.Debug($"Convert {FileToConvert.StringValue}");

            var options = new Options(new string[] { "--inputFile", FileToConvert.StringValue, "--outputDir", "tmp" });

            var _htmlToEpub = new HtmlToEpubConverter(
                new Counter(_log),
                _log,
                options,
                new MecabParser(),
                new MecabReader(),
                new MecabBackend(),
                new XHtmlMaker(),
                new JmdicFastReader(
                    _log,
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
            _log.Debug("end");
        }
    }
}
