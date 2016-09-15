using JpAnnotator.Common.Portable.Gui.MainWindow;
using PropertyChanged;

namespace JpAnnotator.Common.Windows.Gui.MainWindow
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel : IMainWindowViewModel
    {
        public string SourceFile { get; set; }
        public string TargetFile { get; set; }
    }
}
