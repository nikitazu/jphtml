using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using JpAnnotator.Core.Dic;
using JpAnnotator.Core.Format;
using JpAnnotator.Core.Make.Epub;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Logging;
using JpAnnotator.Utils;
using JpAnnotator.Common.Portable.Bundling;

namespace JpAnnotator.Core
{
    public class HtmlToEpubConverter
    {
        readonly Counter _counter;
        readonly ILogWriter _log;
        readonly Options _options;
        readonly IResourceLocator _resourceLocator;
        readonly MecabParser _parser;
        readonly MecabReader _reader;
        readonly MecabBackend _mecabBackend;
        readonly XHtmlMaker _xhtmlMaker;
        readonly JmdicFastReader _dicReader;
        readonly ContentsBreaker _breaker;
        readonly EpubMaker _epubMaker;

        public HtmlToEpubConverter(
            Counter counter,
            ILogWriter log,
            Options options,
            IResourceLocator resourceLocator,
            MecabParser parser,
            MecabReader reader,
            MecabBackend backend,
            XHtmlMaker xhtmlMaker,
            JmdicFastReader dicReader,
            ContentsBreaker breaker,
            EpubMaker epubMaker)
        {
            _counter = counter;
            _log = log;
            _options = options;
            _resourceLocator = resourceLocator;
            _parser = parser;
            _reader = reader;
            _mecabBackend = backend;
            _xhtmlMaker = xhtmlMaker;
            _dicReader = dicReader;
            _breaker = breaker;
            _epubMaker = epubMaker;
        }

        public async Task Convert()
        {
            _log.Debug("Convert start");
            _counter.Start();

            ContentsInfo contents = await Task.Factory.StartNew(CreateAnnotatedXhtmlContents);

            await ConvertXhtmlToEpub(contents).ContinueWith(_ =>
            {
                _counter.Stop();
                _log.Debug("Convert end");
            });
        }

        ContentsInfo CreateAnnotatedXhtmlContents()
        {
            ContentsInfo contents;
            using (var inputReader = new StreamReader(_options.InputFile, Encoding.UTF8))
            {
                contents = _breaker.Analyze(inputReader);
                _breaker.BreakInMemory(_options.InputFile, contents);
            }

            foreach (var chapter in contents.ChapterFiles)
            {
                _log.Debug($"Html for chapter {chapter.Name}");
                AnnotateMappingAndConvertToXhtml(chapter);
            }
            return contents;
        }

        void AnnotateMappingAndConvertToXhtml(ContentsMapping chapterMapping)
        {
            var xhtmlParagraphs = new List<XElement>();
            foreach (var plainTextLine in chapterMapping.PlainTextContent)
            {
                var lines = _reader.ReadResponse(_mecabBackend.ParseText(plainTextLine));
                var words = new List<WordInfo>();

                foreach (var line in lines)
                {
                    var word = _parser.ParseWord(line);
                    word.Translation = _dicReader.Lookup(word.RootForm);
                    words.Add(word);
                }

                xhtmlParagraphs.Add(_xhtmlMaker.MakeParagraph(words.Select(w => _xhtmlMaker.MakeWord(w))));
                xhtmlParagraphs.Add(_xhtmlMaker.MakeContextHelpParagraph(words.DistinctBy(w => w.Text)));
                words.Clear();
            }

            chapterMapping.XhtmlContent = _xhtmlMaker.MakeRootNode(xhtmlParagraphs);
        }

        async Task ConvertXhtmlToEpub(ContentsInfo contents)
        {
            await _epubMaker.ConvertHtmlToEpub(contents);
        }
    }
}

