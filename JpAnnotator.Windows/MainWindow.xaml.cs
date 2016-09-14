using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using JpAnnotator.Common.Portable.Bundling;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Common.Portable.Gui;
using JpAnnotator.Common.Portable.OperatingSystem;
using JpAnnotator.Common.Portable.PlainText;
using JpAnnotator.Common.Windows;
using JpAnnotator.Common.Windows.Gui;
using JpAnnotator.Common.Windows.OperatingSystem;
using JpAnnotator.Core;
using JpAnnotator.Core.Dic;
using JpAnnotator.Core.Make.Epub;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Logging;

namespace JpAnnotator.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly IDialogCreator _dialog;
        readonly INativeFileManager _fileManager;
        readonly IResourceLocator _resourceLocator;
        readonly ILogWriter _log;
        readonly Task<JmdicFastReader> _jmdicReaderTask;
        string _sourceFile;

        public MainWindow()
        {
            InitializeComponent();
            _dialog = new WpfDialogCreator(this);
            _fileManager = new WindowsExplorerFileManager();
            _resourceLocator = new WindowsResourceLocator();
            _log = new LoggingConfig(_resourceLocator).CreateRootLogWriter();
            _log.Debug("start");
            _jmdicReaderTask = Task.Factory.StartNew(() => new JmdicFastReader(
                _log,
                _resourceLocator,
                new Jmdictionary())
            );
        }

        void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            string inputFile;
            if (_dialog.OpenFile("Choose source file", "Text files|*.txt|Markdown files|*.md|All Files|*.*", out inputFile))
            {
                _sourceFile = inputFile;
            }
        }

        async void ConvertButtonClick(object sender, RoutedEventArgs e)
        {
            _log.Debug($"Convert {_sourceFile}");

            if (string.IsNullOrWhiteSpace(_sourceFile))
            {
                _dialog.Info("Source file wasn't chosen", "Conversion is cancelled because the source file wasn't chosen");
                return;
            }

            if (!File.Exists(_sourceFile))
            {
                _dialog.Info("Source file doesn't exist", "Conversion is cancelled because the source file doesn't exist");
                return;
            }

            string outputFile;
            string filename = string.IsNullOrWhiteSpace(_sourceFile) ? string.Empty : Path.GetFileNameWithoutExtension(_sourceFile);
            if (!_dialog.SaveFile("Save epub as file", "Epub files|*.epub", filename, out outputFile))
            {
                _dialog.Info("Target file wasn't chosen", "Conversion is cancelled because the target file wasn't chosen");
                return;
            }

            var options = new Options(new string[] {
                "--inputFile", _sourceFile,
                "--outputFile", outputFile
            });

            // lock ui

            try
            {
                var htmlToEpub = new HtmlToEpubConverter(
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

                await htmlToEpub.Convert();
            }
            catch (Exception ex)
            {
                _dialog.UnexpectedError(ex);
                throw;
            }
            finally
            {
                // unlock ui
                _fileManager.OpenFileManagerAndShowFile(Path.GetDirectoryName(outputFile));
            }
            _log.Debug("end");
        }
    }
}
