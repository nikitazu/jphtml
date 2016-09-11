using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Core.Dic;
using JpAnnotator.Core.Format;
using JpAnnotator.Core.Make.Epub;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Logging;
using JpAnnotator.Utils;
using JpAnnotator.Common.Portable.PlainText;

namespace JpAnnotator.Core
{
    public class HtmlToEpubConverter
    {
        readonly string _inputFile;
        readonly Counter _counter;
        readonly ILogWriter _log;
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
            IOptionProviderInputFile options,
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
            _parser = parser;
            _reader = reader;
            _mecabBackend = backend;
            _xhtmlMaker = xhtmlMaker;
            _dicReader = dicReader;
            _breaker = breaker;
            _epubMaker = epubMaker;
            _inputFile = options.InputFile;
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
            using (var inputReader = new MarkingTextReader(new StreamReader(_inputFile, Encoding.UTF8)))
            {
                contents = _breaker.Analyze(inputReader);
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
                IList<string> lines;
                using (var reader = _mecabBackend.ParseText(plainTextLine))
                {
                    lines = _reader.ReadResponse(reader);
                }
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

