using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using JpAnnotator.Core.Make.Epub;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Logging;
using jphtml.Core;
using jphtml.Core.Dic;
using jphtml.Core.Format;
using jphtml.Utils;
using NMeCab;
using JpAnnotator.Common.Windows;

namespace jphtml
{
    class MainClass
    {
        static Counter _counter;
        static ILogWriter _log;
        static Options _options;
        static MecabParser _parser;
        static MecabReader _reader;
        static MeCabTagger _mecabTagger;
        static XHtmlMaker _xhtmlMaker;
        static JmdicFastReader _dicReader;
        static ContentsBreaker _breaker;

        public static void Main(string[] args)
        {
            _log = new LoggingConfig(new WindowsResourceLocator()).CreateRootLogWriter();
            _log.Debug("start");

            _options = new Options(args);
            _reader = new MecabReader();
            _parser = new MecabParser();
            _mecabTagger = MeCabTagger.Create();
            _xhtmlMaker = new XHtmlMaker();
            _dicReader = new JmdicFastReader(
                _log,
                _options,
                Path.Combine(FileSystemUtils.AppDir, "data", "dic", "JMdict_e"),
                new Jmdictionary()
            );
            _breaker = new ContentsBreaker(
                _options.OutputDir,
                _options.ChapterMarkers
            );
            _counter = new Counter(_log);

            _options.Print();

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
            _log.Debug("end");
        }

        static void ConvertFileToHtml(ContentsMapping chapterMapping)
        {
            var xhtmlParagraphs = new List<XElement>();
            foreach (var plainTextLine in chapterMapping.PlainTextContent)
            {
                var lines = _reader.ReadResponse(new StringReader(_mecabTagger.Parse(plainTextLine)));
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

        static async Task ConvertHtmlToEpub(ContentsInfo contents)
        {
            await new EpubMaker(_log, _options).ConvertHtmlToEpub(contents);
        }
    }
}
