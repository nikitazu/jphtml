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
        readonly IDialogCreator _dialogs;
        string _sourceFile;

        public MainWindow()
        {
            InitializeComponent();
            _dialogs = new WpfDialogCreator();
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            string path;
            if (_dialogs.OpenFile("Choose file to convert", "*.txt", out path))
            {
                _sourceFile = path;
            }
        }

        private void ConvertButtonClick(object sender, RoutedEventArgs e)
        {
            Title = _sourceFile ?? "None";
        }
    }
}
