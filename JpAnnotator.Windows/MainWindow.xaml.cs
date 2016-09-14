using System;
using System.IO;
using System.Windows;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Common.Portable.Gui;
using JpAnnotator.Common.Portable.PlainText;
using JpAnnotator.Common.Windows;
using JpAnnotator.Common.Windows.Gui;
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
        string _sourceFile;

        public MainWindow()
        {
            InitializeComponent();
            _dialog = new WpfDialogCreator(this);
        }

        void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            string path;
            if (_dialog.OpenFile("Choose file to convert", "Text files|*.txt|All Files|*.*", out path))
            {
                _sourceFile = path;
                Title = path ?? "nope";
            }
        }

        async void ConvertButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_sourceFile))
            {
                _dialog.Info("Source file not chosen", "Choose source file to convert");
                return;
            }

            if (!File.Exists(_sourceFile))
            {
                _dialog.Info("Source file not exists", "Choose source file that exists to convert");
                return;
            }

            string targetFile;
            if (!_dialog.SaveFile("Choose file to save", "Epub files|*.epub|All Files|*.*", _sourceFile + ".epub", out targetFile))
            {
                _dialog.Info("Target file not chosen", "Choose target file to convert");
                return;
            }

            _dialog.Info("TODO", $"Converting {_sourceFile} to {targetFile}");

            var resourceLocator = new WindowsResourceLocator();
            var log = new LoggingConfig(resourceLocator).CreateRootLogWriter();
            log.Debug("start");

            var options = new Options(new string[] {
                "--inputFile", _sourceFile,
                "--outputFile", targetFile
            });

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

            await _htmlToEpub.Convert().ContinueWith(_ =>
            {
                log.Debug("end");
            });
        }
    }
}
