using System.Diagnostics;
using System.IO;
using JpAnnotator.Common.Portable.OperatingSystem;

namespace JpAnnotator.Common.Mac.OperatingSystem
{
    public class FinderFileManager : INativeFileManager
    {
        void INativeFileManager.OpenFileManagerAndShowFile(string path)
        {
            if (File.Exists(path))
            {
                Process.Start("/usr/bin/open", $"--reveal {path}");
            }
        }
    }
}
