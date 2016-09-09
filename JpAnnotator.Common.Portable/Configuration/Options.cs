using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JpAnnotator.Common.Portable.Configuration
{
    public class Options :
        IOptionProviderInputFile,
        IOptionProviderEpub,
        IOptionProviderChapterMarkers
    {
        readonly string _inputFile = string.Empty;
        readonly string _outputFile = string.Empty;
        readonly IReadOnlyList<string> _chapterMarkers = new List<string>();
        readonly string _author = "Unknown";
        readonly string _bookId = Guid.NewGuid().ToString();
        readonly string _publisher = "Unknown";

        string IOptionProviderInputFile.InputFile => _inputFile;
        string IOptionProviderEpub.Author => _author;
        string IOptionProviderEpub.BookId => _bookId;
        string IOptionProviderEpub.Publisher => _publisher;
        string IOptionProviderEpub.OutputFile => _outputFile;
        IReadOnlyList<string> IOptionProviderChapterMarkers.ChapterMarkers => _chapterMarkers;

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
    }
}
