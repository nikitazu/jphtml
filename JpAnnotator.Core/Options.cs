using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace JpAnnotator.Core
{
    public class Options :
        IOptionProvider<IOptionConsumerInputFile>,
        IOptionProvider<IOptionConsumerEpub>,
        IOptionProvider<IOptionConsumerChapterMarkers>
    {
        readonly string _inputFile = string.Empty;
        readonly string _outputFile = string.Empty;
        readonly IReadOnlyList<string> _chapterMarkers = new List<string>();
        readonly string _author = "Unknown";
        readonly string _bookId = Guid.NewGuid().ToString();
        readonly string _publisher = "Unknown";

        public Options(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                string key = args[i++];
                string value = args[i];

                switch (key)
                {
                    case "--inputFile": _inputFile = value; break;
                    case "--outputFile": _outputFile = value; break;
                    case "--chapterMarkers": _chapterMarkers = value.Split(',').ToArray(); break;
                    case "--author": _author = value; break;
                    case "--bookId": _bookId = value; break;
                    case "--publisher": _publisher = value; break;
                }
            }
        }

        public void Print(TextWriter writer)
        {
            writer.WriteLine($"Input file: {_inputFile}");
            writer.WriteLine($"Output file: {_outputFile}");
            writer.WriteLine($"Chapter markers: {string.Join(",", _chapterMarkers)}");
            writer.WriteLine($"Author: {_author}");
            writer.WriteLine($"Book id: {_bookId}");
            writer.WriteLine($"Publisher: {_publisher}");
        }

        void IOptionProvider<IOptionConsumerInputFile>.Provide(IOptionConsumerInputFile consumer)
        {
            consumer.Consume(_inputFile);
        }

        void IOptionProvider<IOptionConsumerEpub>.Provide(IOptionConsumerEpub consumer)
        {
            consumer.Consume(_author, _bookId, _publisher, _outputFile);
        }

        void IOptionProvider<IOptionConsumerChapterMarkers>.Provide(IOptionConsumerChapterMarkers consumer)
        {
            consumer.Consume(_chapterMarkers);
        }
    }

    public interface IOptionProvider<T>
    {
        void Provide(T consumer);
    }

    public interface IOptionConsumerInputFile
    {
        void Consume(string inputFile);
    }

    public interface IOptionConsumerEpub
    {
        void Consume(string author, string bookId, string publisher, string outputFile);
    }

    public interface IOptionConsumerChapterMarkers
    {
        void Consume(IReadOnlyList<string> chapterMarkers);
    }
}
