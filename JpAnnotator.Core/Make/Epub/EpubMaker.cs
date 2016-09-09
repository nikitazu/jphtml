using System;
using System.IO;
using System.Threading.Tasks;
using EPubFactory;
using JpAnnotator.Common.Portable.Bundling;
using JpAnnotator.Core.Format;
using JpAnnotator.Logging;

namespace JpAnnotator.Core.Make.Epub
{
    public class EpubMaker : IOptionConsumerEpub
    {
        string _author;
        string _bookId;
        string _publisher;
        string _outputFile;

        readonly ILogWriter _log;
        readonly IResourceLocator _resourceLocator;

        public EpubMaker(
            ILogWriter log,
            IOptionProvider<IOptionConsumerEpub> options,
            IResourceLocator resourceLocator)
        {
            _log = log;
            _resourceLocator = resourceLocator;
            options.Provide(this);
        }

        void IOptionConsumerEpub.Consume(string author, string bookId, string publisher, string outputFile)
        {
            _author = author;
            _bookId = bookId;
            _publisher = publisher;
            _outputFile = outputFile;
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
