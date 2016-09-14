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
            string path;
            if (_dialog.SaveFile("Choose file to save", "Epub files|*.epub|All Files|*.*", _sourceFile + ".epub", out path))
            {
                Title = path ?? "None";
            }
        }
    }
}
