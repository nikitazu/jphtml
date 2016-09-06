using System.IO;
using System.Threading.Tasks;
using EPubFactory;
using JpAnnotator.Common.Portable.Bundling;
using JpAnnotator.Core.Format;
using JpAnnotator.Logging;

namespace JpAnnotator.Core.Make.Epub
{
    public class EpubMaker
    {
        readonly ILogWriter _log;
        readonly Options _options;
        readonly IResourceLocator _resourceLocator;

        public EpubMaker(
            ILogWriter log,
            Options options,
            IResourceLocator resourceLocator)
        {
            _log = log;
            _options = options;
            _resourceLocator = resourceLocator;
        }

        public async Task ConvertHtmlToEpub(ContentsInfo contents)
        {
            _log.Debug("epub start");
            using (var epubStream = File.Create(_options.OutputFile))
            using (var epubWriter = await EPubWriter.CreateWriterAsync(
                epubStream,
                Path.GetFileNameWithoutExtension(_options.OutputFile),
                _options.Author,
                _options.BookId))
            {
                epubWriter.Publisher = _options.Publisher;
                foreach (var chapter in contents.ChapterFiles)
                {
                    await epubWriter.AddChapterAsync(
                        chapter.Name + ".html",
                        Path.GetFileNameWithoutExtension(chapter.Name),
                        chapter.XhtmlContent.ToString());
                }
                await epubWriter.AddResourceAsync(
                    "style.css",
                    "text/css",
                    File.ReadAllBytes(Path.Combine(_resourceLocator.ResourcesPath, "data", "epub", "style.css")));

                await epubWriter.WriteEndOfPackageAsync().ContinueWith(_ =>
                {
                    _log.Debug("epub done");
                });
            }
        }
    }
}
