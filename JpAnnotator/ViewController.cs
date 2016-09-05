using System;
using AppKit;
using Foundation;
using JpAnnotator.Logging;
using JpAnnotator.Common.Mac;
using JpAnnotator.Core;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Core.Dic;
using System.IO;
using System.Resources;
using JpAnnotator.Common.Portable.Bundling;
using JpAnnotator.Core.Make.Epub;

namespace JpAnnotator
{
    public partial class ViewController : NSViewController
    {
        readonly IResourceLocator _resourceLocator;
        readonly ILogWriter _log;

        public ViewController(IntPtr handle) : base(handle)
        {
            _resourceLocator = new MacResourceLocator();
            _log = new LoggingConfig(_resourceLocator).CreateRootLogWriter();
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

            var options = new Options(new string[]
            {
                "--inputFile",  FileToConvert.StringValue,
                "--outputDir", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "tmp"),
                "--chapterMarkers", "第1章,第2章,第3章,第4章,第5章,第6章,第7章,第8章,第9章,第10章,第11章,第12章,第13章,第14章,第15章,第16章,第17章,第18章,第19章,第20章,第21章,第22章,第23章,第24"
            });

            var _htmlToEpub = new HtmlToEpubConverter(
                new Counter(_log),
                _log,
                options,
                _resourceLocator,
                new MecabParser(),
                new MecabReader(),
                new MecabBackend(),
                new XHtmlMaker(),
                new JmdicFastReader(
                    _log,
                    options,
                    _resourceLocator,
                    new Jmdictionary()
                ),
                new ContentsBreaker(
                    options.OutputDir,
                    options.ChapterMarkers
                ),
                new EpubMaker(_log, options, _resourceLocator)
            );

            options.Print();

            _htmlToEpub.Convert().ContinueWith(_ =>
            {
                _log.Debug("end");
            });
        }
    }
}
