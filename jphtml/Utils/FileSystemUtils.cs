using System.IO;
using System.Reflection;

namespace jphtml
{
    public static class FileSystemUtils
    {
        public static string AppDir => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
}

