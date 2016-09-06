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
using System.Threading.Tasks;

namespace JpAnnotator
{
    public partial class ViewController : NSViewController
    {
        readonly IResourceLocator _resourceLocator;
        readonly ILogWriter _log;
        readonly Task<JmdicFastReader> _jmdicReaderTask;

        public ViewController(IntPtr handle) : base(handle)
        {
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
            if (OpenFileDialog("Choose source file", out inputFile))
            {
                FileToConvert.StringValue = inputFile;
            }
        }

        async partial void ConvertButtonClicked(NSObject sender)
        {
            _log.Debug($"Convert {FileToConvert.StringValue}");

            if (string.IsNullOrWhiteSpace(FileToConvert.StringValue))
            {
                InfoDialog("Source file wasn't chosen", "Conversion is cancelled because the source file wasn't chosen");
                return;
            }

            if (!File.Exists(FileToConvert.StringValue))
            {
                InfoDialog("Source file doesn't exist", "Conversion is cancelled because the source file doesn't exist");
                return;
            }

            string outputFile;
            string filename = string.IsNullOrWhiteSpace(FileToConvert.StringValue) ? string.Empty : Path.GetFileNameWithoutExtension(FileToConvert.StringValue);
            if (!SaveFileDialog("Save epub as file", filename, out outputFile))
            {
                InfoDialog("Target file wasn't chosen", "Conversion is cancelled because the target file wasn't chosen");
                return;
            }

            var options = new Options(new string[]
            {
                "--inputFile",  FileToConvert.StringValue,
                "--outputFile", outputFile,
                "--chapterMarkers", "第1章,第2章,第3章,第4章,第5章,第6章,第7章,第8章,第9章,第10章,第11章,第12章,第13章,第14章,第15章,第16章,第17章,第18章,第19章,第20章,第21章,第22章,第23章,第24"
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
                    _resourceLocator,
                    new MecabParser(),
                    new MecabReader(),
                    new MecabBackend(),
                    new XHtmlMaker(),
                    await _jmdicReaderTask,
                    new ContentsBreaker(options),
                    new EpubMaker(_log, options, _resourceLocator)
                );

                options.Print();

                await _htmlToEpub.Convert();
            }
            catch (Exception ex)
            {
                UnexpectedError(ex);
            }
            finally
            {
                OpenButton.Enabled = true;
                ConvertButton.Enabled = true;
                FileToConvert.Enabled = true;
                ConversionStatus.StringValue = "Conversion done";
                ConversionProgress.StopAnimation(null);
            }

            _log.Debug("end");
        }

        bool OpenFileDialog(string title, out string path)
        {
            path = null;

            var dlg = NSOpenPanel.OpenPanel;
            dlg.Title = title;
            dlg.CanChooseFiles = true;
            dlg.CanChooseDirectories = false;
            dlg.AllowedFileTypes = new string[] { "txt", "md" };

            if (dlg.RunModal() == 1)
            {
                var url = dlg.Urls[0];

                if (url != null)
                {
                    path = url.Path;
                }
            }

            return path != null;
        }

        bool SaveFileDialog(string title, string filename, out string path)
        {
            path = null;

            var dlg = new NSSavePanel();
            dlg.Title = title;
            dlg.AllowedFileTypes = new string[] { "epub" };

            if (!string.IsNullOrWhiteSpace(filename))
            {
                dlg.NameFieldStringValue = filename;
            }

            if (dlg.RunModal() == 1 && dlg.Url != null)
            {
                path = dlg.Url.Path;
            }

            return path != null;
        }

        void InfoDialog(string title, string message)
        {
            new NSAlert
            {
                AlertStyle = NSAlertStyle.Informational,
                InformativeText = message,
                MessageText = title,
            }.RunModal();
        }

        void UnexpectedError(Exception ex)
        {
            new NSAlert
            {
                AlertStyle = NSAlertStyle.Critical,
                InformativeText = ex.ToString(),
                MessageText = "Unexpected error",
            }.RunModal();
        }
    }
}
