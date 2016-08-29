using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        static ILogWriter _log;
        static Options _options;
        static MecabRunner _runner;
        static MecabParser _parser;
        static MecabReader _reader;
        static HtmlSimplePrinter _printer;
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
            _printer = new HtmlSimplePrinter();
            _dicReader = new JmdicFastReader(
                _log,
                _options,
                Path.Combine(FileSystemUtils.AppDir, "data", "dic", "JMdict_e"),
                new Jmdictionary()
            );
            _breaker = new ContentsBreaker(_options.ChapterMarkers);

            _options.Print();

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

                int iteration = 0;
                pipeline.Run((fileReader, fileWriter) =>
                {
                    if (iteration == 0)
                    {
                        _printer.PrintDocumentBegin(fileWriter);
                    }

                    process.StandardInput.WriteLine(fileReader.ReadLine());
                    var lines = _reader.ReadResponse(process.StandardOutput);

                    _log.Debug($"Write html paragraph {iteration}");
                    bool isNewParagraph = true;
                    var words = new List<WordInfo>();
                    foreach (var line in lines)
                    {
                        var word = _parser.ParseWord(line);
                        words.Add(word);

                        if (isNewParagraph)
                        {
                            _printer.PrintParagraphBegin(fileWriter);
                            isNewParagraph = false;
                        }

                        word.Translation = _dicReader.Lookup(word.RootForm);

                        _printer.PrintWord(fileWriter, word);

                        if (word.Text.Equals("。"))
                        {
                            _printer.PrintParagraphEnd(fileWriter);
                            _printer.PrintContextHelp(fileWriter, words);
                            isNewParagraph = true;
                            words.Clear();
                        }
                    }

                    if (fileReader.EndOfStream)
                    {
                        _printer.PrintDocumentEnd(fileWriter);
                    }

                    iteration++;
                });
            });
        }
    }
}
