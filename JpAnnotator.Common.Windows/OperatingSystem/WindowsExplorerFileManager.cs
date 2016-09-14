using System.Diagnostics;
using System.IO;
using JpAnnotator.Common.Portable.OperatingSystem;

namespace JpAnnotator.Common.Windows.OperatingSystem
{
    public class WindowsExplorerFileManager : INativeFileManager
    {
        void INativeFileManager.OpenFileManagerAndShowFile(string path)
        {
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
        }
    }
}
