using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using jphtml.Core;
using jphtml.Core.Dic;
using jphtml.Core.Format;
using jphtml.Core.Html;
using jphtml.Core.IO;
using jphtml.Core.Ipc;
using jphtml.Logging;

namespace jphtml
{
    class MainClass
    {
        static Counter _counter;
        static ILogWriter _log;
        static Options _options;
        static MecabRunner _runner;
        static MecabParser _parser;
        static MecabReader _reader;
        static XHtmlMaker _xhtmlMaker;
        static JmdicFastReader _dicReader;
        static ContentsBreaker _breaker;

        public static void Main(string[] args)
        {
            _log = LoggingConfig.CreateRootLogWriter();
            _log.Debug("start");

            _options = new Options(args);
            _runner = new MecabRunner();
            _reader = new MecabReader();
            _parser = new MecabParser();
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
            }

            _counter.Stop();
            _log.Debug("end");
        }

        static void ConvertFileToHtml(FilePipeLine pipeline)
        {
            _runner.RunMecab(process =>
            {
                process.ErrorDataReceived += (sender, e) =>
                {
                    _log.Error("ERROR " + process.StandardError.ReadToEnd());
                };

                process.Exited += (sender, e) =>
                {
                    _log.Error($"MeCab EXIT {process.ExitCode}");
                };

                var xhtmlParagraphs = new List<XElement>();

                int iteration = 0;
                pipeline.Run((fileReader, fileWriter) =>
                {
                    process.StandardInput.WriteLine(fileReader.ReadLine());
                    var lines = _reader.ReadResponse(process.StandardOutput);

                    //_log.Debug($"Write html paragraph {iteration}");
                    var xhtmlWordNodes = new List<XNode>();
                    foreach (var line in lines)
                    {
                        var word = _parser.ParseWord(line);
                        word.Translation = _dicReader.Lookup(word.RootForm);

                        xhtmlWordNodes.Add(_xhtmlMaker.MakeWord(word));
                    }

                    xhtmlParagraphs.Add(_xhtmlMaker.MakeParagraph(xhtmlWordNodes));
                    xhtmlWordNodes.Clear();

                    if (fileReader.EndOfStream)
                    {
                        var doc = _xhtmlMaker.MakeRootNode(xhtmlParagraphs);
                        using (var xwr = new XmlTextWriter(fileWriter))
                        {
                            doc.WriteTo(xwr);
                        }
                    }

                    iteration++;
                });
            });
        }
    }
}
