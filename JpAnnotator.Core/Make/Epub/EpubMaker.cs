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
            var fileName = Path.GetFileNameWithoutExtension(_options.InputFile);
            var epub = File.Create(Path.Combine(_options.OutputDir, fileName + ".epub"));
            using (var writer = await EPubWriter.CreateWriterAsync(
                epub,
                fileName,
                _options.Author,
                _options.BookId))
            {
                writer.Publisher = _options.Publisher;
                foreach (var chapter in contents.ChapterFiles)
                {
                    await writer.AddChapterAsync(
                        Path.GetFileName(chapter.FilePath + ".html"),
                        Path.GetFileNameWithoutExtension(chapter.FilePath),
                        chapter.XhtmlContent.ToString());
                }
                await writer.AddResourceAsync(
                    "style.css",
                    "text/css",
                    File.ReadAllBytes(Path.Combine(_resourceLocator.ResourcesPath, "data", "epub", "style.css")));
                await writer.WriteEndOfPackageAsync();
            }
            _log.Debug("epub done");
        }
    }
}
