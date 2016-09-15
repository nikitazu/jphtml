using System.IO;
using System.Reflection;
using JpAnnotator.Common.Portable.Bundling;

namespace JpAnnotator.Common.Bundling
{
    public class ConsoleAppResourceLocator : IResourceLocator
    {
        string IResourceLocator.ResourcesPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
}
