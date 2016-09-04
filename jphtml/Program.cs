using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EPubFactory;
using jphtml.Core;
using jphtml.Core.Dic;
using jphtml.Core.Format;
using jphtml.Core.Html;
using jphtml.Core.IO;
using jphtml.Core.Ipc;
using jphtml.Logging;
using jphtml.Utils;
using NMeCab;

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
            _log = LoggingConfig.CreateRootLogWriter();
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

            ContentsInfo contents;
            using (var inputReader = new StreamReader(_options.InputFile, Encoding.UTF8))
            {
                contents = _breaker.Analyze(inputReader);
                _breaker.Break(_options.InputFile, contents);
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
                    var pipeline = new FilePipeLine(_log, chapter.FilePath, chapter.FilePath + ".html");
                    ConvertFileToHtml(pipeline);
                }
                File.Copy(
                    Path.Combine(FileSystemUtils.AppDir, "data", "html", "style.css"),
                    Path.Combine(_options.OutputDir, "style.css"));
            }

            ConvertHtmlToEpub(contents).Wait();

            _counter.Stop();
            _log.Debug("end");
        }

        static void ConvertFileToHtml(FilePipeLine pipeline)
        {
            var xhtmlParagraphs = new List<XElement>();
            pipeline.Run((fileReader, fileWriter) =>
            {
                var lines = _reader.ReadResponse(new StringReader(_mecabTagger.Parse(fileReader.ReadLine())));
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

                if (fileReader.EndOfStream)
                {
                    var doc = _xhtmlMaker.MakeRootNode(xhtmlParagraphs);
                    using (var xwr = new XmlTextWriter(fileWriter))
                    {
                        doc.WriteTo(xwr);
                    }
                }
            });
        }

        static async Task ConvertHtmlToEpub(ContentsInfo contents)
        {
            _log.Debug("epub start");
            var fileName = Path.GetFileNameWithoutExtension(_options.InputFile);
            var epub = File.Create(Path.Combine(_options.OutputDir, fileName + ".epub"));
            using (var writer = await EPubWriter.CreateWriterAsync(
                epub,
                fileName,
                "Megaman",
                "123123"))
            {
                writer.Publisher = "Zuich";
                foreach (var chapter in contents.ChapterFiles)
                {
                    await writer.AddChapterAsync(
                        Path.GetFileName(chapter.FilePath + ".html"),
                        Path.GetFileNameWithoutExtension(chapter.FilePath),
                        File.ReadAllText(chapter.FilePath + ".html"));
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
