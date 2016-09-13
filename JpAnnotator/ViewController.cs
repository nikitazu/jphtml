using System;
using System.IO;
using System.Threading.Tasks;
using AppKit;
using Foundation;
using JpAnnotator.Common.Mac;
using JpAnnotator.Common.Mac.OperatingSystem;
using JpAnnotator.Common.Portable.Bundling;
using JpAnnotator.Common.Portable.Gui;
using JpAnnotator.Common.Portable.OperatingSystem;
using JpAnnotator.Core;
using JpAnnotator.Core.Dic;
using JpAnnotator.Core.Make.Epub;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Logging;
using JpAnnotator.Common.Mac.Gui;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Common.Portable.PlainText;

namespace JpAnnotator
{
    public partial class ViewController : NSViewController
    {
        readonly IDialogCreator _dialog;
        readonly INativeFileManager _finder;
        readonly IResourceLocator _resourceLocator;
        readonly ILogWriter _log;
        readonly Task<JmdicFastReader> _jmdicReaderTask;

        public ViewController(IntPtr handle) : base(handle)
        {
            _dialog = new CocoaDialogCreator();
            _finder = new FinderFileManager();
            _resourceLocator = new MacResourceLocator();
            _log = new LoggingConfig(_resourceLocator).CreateRootLogWriter();
            _log.Debug("start");
            _jmdicReaderTask = Task.Factory.StartNew(() => new JmdicFastReader(
                _log,
                _resourceLocator,
                new Jmdictionary()
            ));
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
            string inputFile;
            if (_dialog.OpenFile("Choose source file", "txt,md", out inputFile))
            {
                FileToConvert.StringValue = inputFile;
            }
        }

        async partial void ConvertButtonClicked(NSObject sender)
        {
            _log.Debug($"Convert {FileToConvert.StringValue}");

            if (string.IsNullOrWhiteSpace(FileToConvert.StringValue))
            {
                _dialog.Info("Source file wasn't chosen", "Conversion is cancelled because the source file wasn't chosen");
                return;
            }

            if (!File.Exists(FileToConvert.StringValue))
            {
                _dialog.Info("Source file doesn't exist", "Conversion is cancelled because the source file doesn't exist");
                return;
            }

            string outputFile;
            string filename = string.IsNullOrWhiteSpace(FileToConvert.StringValue) ? string.Empty : Path.GetFileNameWithoutExtension(FileToConvert.StringValue);
            if (!_dialog.SaveFile("Save epub as file", "epub", filename, out outputFile))
            {
                _dialog.Info("Target file wasn't chosen", "Conversion is cancelled because the target file wasn't chosen");
                return;
            }

            var options = new Options(new string[]
            {
                "--inputFile",  FileToConvert.StringValue,
                "--outputFile", outputFile,
            });

            OpenButton.Enabled = false;
            ConvertButton.Enabled = false;
            FileToConvert.Enabled = false;
            ConversionStatus.StringValue = "Conversion started...";
            ConversionProgress.StartAnimation(null);

            try
            {
                var _htmlToEpub = new HtmlToEpubConverter(
                    new Counter(_log),
                    _log,
                    options,
                    new MecabParser(),
                    new MecabReader(),
                    new MecabBackend(),
                    new XHtmlMaker(),
                    await _jmdicReaderTask,
                    new ContentsBreaker(new ChapterMarkersProvider(options, new ContentsDetector())),
                    new EpubMaker(_log, options, _resourceLocator),
                    new SentenceBreaker()
                );

                options.Print(Console.Out);

                await _htmlToEpub.Convert();
            }
            catch (Exception ex)
            {
                _dialog.UnexpectedError(ex);
            }
            finally
            {
                OpenButton.Enabled = true;
                ConvertButton.Enabled = true;
                FileToConvert.Enabled = true;
                ConversionStatus.StringValue = "Conversion done";
                ConversionProgress.StopAnimation(null);

                _finder.OpenFileManagerAndShowFile(outputFile);
            }

            _log.Debug("end");
        }
    }
}
