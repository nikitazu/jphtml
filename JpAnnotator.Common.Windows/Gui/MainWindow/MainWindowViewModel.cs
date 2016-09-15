using System.ComponentModel;
using JpAnnotator.Common.Portable.Gui.MainWindow;

namespace JpAnnotator.Common.Windows.Gui.MainWindow
{
    public class MainWindowViewModel : IMainWindowViewModel, INotifyPropertyChanged
    {
        private string _sourceFile;
        private string _targetFile;

        public string SourceFile
        {
            get { return _sourceFile; }
            set
            {
                if (_sourceFile != value)
                {
                    _sourceFile = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceFile)));
                }
            }
        }

        public string TargetFile
        {
            get { return _targetFile; }
            set
            {
                if (_targetFile != value)
                {
                    _targetFile = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetFile)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
