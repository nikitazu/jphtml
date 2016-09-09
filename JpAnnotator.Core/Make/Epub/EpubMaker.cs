using System.IO;
using System.Threading.Tasks;
using EPubFactory;
using JpAnnotator.Common.Portable.Bundling;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Core.Format;
using JpAnnotator.Logging;

namespace JpAnnotator.Core.Make.Epub
{
    public class EpubMaker
    {
        readonly string _author;
        readonly string _bookId;
        readonly string _publisher;
        readonly string _outputFile;

        readonly ILogWriter _log;
        readonly IResourceLocator _resourceLocator;

        public EpubMaker(
            ILogWriter log,
            IOptionProviderEpub options,
            IResourceLocator resourceLocator)
        {
            _log = log;
            _resourceLocator = resourceLocator;
            _author = options.Author;
            _bookId = options.BookId;
            _publisher = options.Publisher;
            _outputFile = options.OutputFile;
        }

        public async Task ConvertHtmlToEpub(ContentsInfo contents)
        {
            _log.Debug("epub start");
            using (var epubStream = File.Create(_outputFile))
            using (var epubWriter = await EPubWriter.CreateWriterAsync(
                epubStream,
                Path.GetFileNameWithoutExtension(_outputFile),
                _author,
                _bookId))
            {
                epubWriter.Publisher = _publisher;
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
