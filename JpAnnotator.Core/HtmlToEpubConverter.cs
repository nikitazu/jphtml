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

namespace JpAnnotator.Core
{
    public class HtmlToEpubConverter
    {
        readonly Counter _counter;
        readonly ILogWriter _log;
        readonly Options _options;
        readonly MecabParser _parser;
        readonly MecabReader _reader;
        readonly MecabBackend _mecabBackend;
        readonly XHtmlMaker _xhtmlMaker;
        readonly JmdicFastReader _dicReader;
        readonly ContentsBreaker _breaker;

        public HtmlToEpubConverter(
            Counter counter,
            ILogWriter log,
            Options options,
            MecabParser parser,
            MecabReader reader,
            MecabBackend backend,
            XHtmlMaker xhtmlMaker,
            JmdicFastReader dicReader,
            ContentsBreaker breaker)
        {
            _counter = counter;
            _log = log;
            _options = options;
            _parser = parser;
            _reader = reader;
            _mecabBackend = backend;
            _xhtmlMaker = xhtmlMaker;
            _dicReader = dicReader;
            _breaker = breaker;
        }

        public void Convert()
        {
            _log.Debug("Convert start");
            _counter.Start();

            if (Directory.Exists(_options.OutputDir))
            {
                Directory.Delete(_options.OutputDir, recursive: true);
            }
            Directory.CreateDirectory(_options.OutputDir);

            ContentsInfo contents;
            using (var inputReader = new StreamReader(_options.InputFile, Encoding.UTF8))
            {
                contents = _breaker.Analyze(inputReader);
                _breaker.BreakInMemory(_options.InputFile, contents);
            }

            if (_log.IsDebug)
            {
                _log.Debug("Chapter mapping");
                foreach (var chapter in contents.ChapterFiles)
                {
                    _log.Debug($"Chapter {chapter.FilePath} [{chapter.StartLine}, {chapter.LengthInLines}]");
                }
            }

            if (!_options.Simulation)
            {
                foreach (var chapter in contents.ChapterFiles)
                {
                    _log.Debug($"Html for chapter {chapter.FilePath}");
                    ConvertFileToHtml(chapter);
                }
                File.Copy(
                    Path.Combine(FileSystemUtils.AppDir, "data", "html", "style.css"),
                    Path.Combine(_options.OutputDir, "style.css"),
                    overwrite: true);
            }

            ConvertHtmlToEpub(contents).Wait();

            _counter.Stop();
            _log.Debug("Convert end");
        }

        void ConvertFileToHtml(ContentsMapping chapterMapping)
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

        async Task ConvertHtmlToEpub(ContentsInfo contents)
        {
            await new EpubMaker(_log, _options).ConvertHtmlToEpub(contents);
        }
    }
}

