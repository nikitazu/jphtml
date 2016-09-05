using System.IO;
using System.Reflection;
using JpAnnotator.Common.Portable.Bundling;

namespace JpAnnotator.Common.Mac
{
    public class MacResourceLocator : IResourceLocator
    {
        string IResourceLocator.ResourcesPath => Path.Combine(
            Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)),
            "Resources");
    }
}
