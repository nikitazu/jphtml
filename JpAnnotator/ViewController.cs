using System;
using System.IO;
using AppKit;
using Foundation;
using JpAnnotator.Common.Mac;
using JpAnnotator.Common.Portable.Bundling;
using JpAnnotator.Core;
using JpAnnotator.Core.Dic;
using JpAnnotator.Core.Make.Epub;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Logging;

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

        partial void OpenButtonClicked(NSObject sender)
        {
            _log.Debug("open file");

            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = true;
            dlg.CanChooseDirectories = false;
            dlg.AllowedFileTypes = new string[] { "txt", "md" };

            if (dlg.RunModal() == 1)
            {
                // Nab the first file
                var url = dlg.Urls[0];

                if (url != null)
                {
                    FileToConvert.StringValue = url.Path;
                }
            }

            _log.Debug("open file end");
        }

        partial void ConvertButtonClicked(NSObject sender)
        {
            _log.Debug($"Convert {FileToConvert.StringValue}");

            var dlg = new NSSavePanel();
            dlg.Title = "Save Text File";
            dlg.AllowedFileTypes = new string[] { "epub" };
            if (dlg.RunModal() == 1)
            {

            }
            else
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = "Conversion is cancelled because the target file wasn't chosen",
                    MessageText = "Target file wasn't chosen",
                };
                alert.RunModal();
                return;
            }

            if (string.IsNullOrWhiteSpace(FileToConvert.StringValue))
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = "Conversion is cancelled because the source file wasn't chosen",
                    MessageText = "Source file wasn't chosen",
                };
                alert.RunModal();
                return;
            }

            if (!File.Exists(FileToConvert.StringValue))
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = "Conversion is cancelled because the source file doesn't exist",
                    MessageText = "Source file doesn't exist",
                };
                alert.RunModal();
                return;
            }

            var outputDir = Path.GetDirectoryName(dlg.Url.Path);

            var options = new Options(new string[]
            {
                "--inputFile",  FileToConvert.StringValue,
                "--outputDir", Path.Combine(outputDir, Path.GetFileNameWithoutExtension(FileToConvert.StringValue)),
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
