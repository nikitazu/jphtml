using System;
using System.Threading.Tasks;
using JpAnnotator.Logging;
using JpAnnotator.Core.Format;
using System.IO;
using EPubFactory;
using JpAnnotator.Core;
using JpAnnotator;

namespace JpAnnotator.Core.Make.Epub
{
    public class EpubMaker
    {
        readonly ILogWriter _log;
        readonly Options _options;

        public EpubMaker(ILogWriter log, Options options)
        {
            _log = log;
            _options = options;
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
                    File.ReadAllBytes(Path.Combine(FileSystemUtils.AppDir, "data", "epub", "style.css")));
                await writer.WriteEndOfPackageAsync();
            }
            _log.Debug("epub done");
        }
    }
}
