using System.IO;
using System.Reflection;

namespace JpAnnotator
{
    public static class FileSystemUtils
    {
        public static string AppDir => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
}

