using System;
using System.Collections.Generic;
using jphtml.Core;
using jphtml.Core.Ipc;
using jphtml.Core.Html;
using jphtml.Core.Format;
using jphtml.Core.Dic;
using jphtml.Core.IO;

namespace jphtml
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("start");

            var options = new Options(args);
            var runner = new MecabRunner();
            var reader = new MecabReader();
            var parser = new MecabParser();
            var printer = new HtmlSimplePrinter();
            var dicReader = new JmdicFastReader("../../../data/dic/JMdict_e", new Jmdictionary());
            var filePipeLine = new FilePipeLine(options.InputFile, options.OutputFile);

            options.Print();

            runner.RunMecab(process =>
            {
                process.ErrorDataReceived += (sender, e) =>
                {
                    Console.WriteLine("ERROR " + process.StandardError.ReadToEnd());
                };

                process.Exited += (sender, e) =>
                {
                    Console.WriteLine($"MeCab EXIT {process.ExitCode}");
                };

                int iteration = 0;
                filePipeLine.Run((fileReader, fileWriter) =>
                {
                    if (iteration == 0)
                    {
                        printer.PrintDocumentBegin(fileWriter);
                    }

                    Console.WriteLine("Send response");
                    process.StandardInput.WriteLine(fileReader.ReadLine());

                    Console.WriteLine("Get response");
                    var lines = reader.ReadResponse(process.StandardOutput);
                    Console.WriteLine(string.Join("\n", lines));

                    Console.WriteLine("Write html");
                    bool isNewParagraph = true;
                    var words = new List<WordInfo>();
                    foreach (var line in lines)
                    {
                        var word = parser.ParseWord(line);
                        words.Add(word);

                        if (isNewParagraph)
                        {
                            printer.PrintParagraphBegin(fileWriter);
                            isNewParagraph = false;
                        }

                        word.Translation = dicReader.Lookup(word.RootForm);

                        printer.PrintWord(fileWriter, word);

                        if (word.Text.Equals("。"))
                        {
                            printer.PrintParagraphEnd(fileWriter);
                            printer.PrintContextHelp(fileWriter, words);
                            isNewParagraph = true;
                            words.Clear();
                        }
                    }

                    if (fileReader.EndOfStream)
                    {
                        printer.PrintDocumentEnd(fileWriter);
                    }

                    iteration++;
                });
            });

            Console.WriteLine("end");
        }
    }
}
