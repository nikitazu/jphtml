using System.IO;
using System.Reflection;
using JpAnnotator.Common.Portable.Bundling;

namespace JpAnnotator.Common.Windows
{
    public class WindowsResourceLocator : IResourceLocator
    {
        string IResourceLocator.ResourcesPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
}
