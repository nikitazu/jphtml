using System.IO;
using System.Windows;
using JpAnnotator.Common.Portable.Gui;
using JpAnnotator.Common.Windows.Gui;

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

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            string path;
            if (_dialog.OpenFile("Choose file to convert", "Text files|*.txt|All Files|*.*", out path))
            {
                _sourceFile = path;
                Title = path ?? "nope";
            }
        }

        private void ConvertButtonClick(object sender, RoutedEventArgs e)
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

            string path;
            if (!_dialog.SaveFile("Choose file to save", "Epub files|*.epub|All Files|*.*", _sourceFile + ".epub", out path))
            {
                _dialog.Info("Target file not chosen", "Choose target file to convert");
                return;
            }

            _dialog.Info("TODO", $"Converting {_sourceFile} to {path}");
        }
    }
}
