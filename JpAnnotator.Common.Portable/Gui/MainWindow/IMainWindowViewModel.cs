namespace JpAnnotator.Common.Portable.Gui.MainWindow
{
    public interface IMainWindowViewModel
    {
        string SourceFile { get; set; }
        string TargetFile { get; set; }
    }
}
