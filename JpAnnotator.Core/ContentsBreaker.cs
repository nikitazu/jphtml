using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Core.Format;

namespace JpAnnotator.Core
{
    public class ContentsBreaker
    {
        readonly IReadOnlyList<string> _chapterMarkers;
        readonly List<string> _chaptersData = new List<string>();

        public ContentsBreaker(IOptionProviderChapterMarkers options)
        {
            _chapterMarkers = options.ChapterMarkers;
        }

        public ContentsInfo Analyze(TextReader reader)
        {
            var contents = new ContentsInfo
            {
                ChapterFiles = new List<ContentsMapping>(_chapterMarkers.Count + 1)
            };

            var counts = _chapterMarkers.Select((marker, i) => CountLinesUntilMarker(reader, marker, i)).ToArray();

            int startLine = 0;
            int chapterIndex = 0;
            for (chapterIndex = 0; chapterIndex < counts.Length; chapterIndex++)
            {
                contents.ChapterFiles.Add(new ContentsMapping()
                {
                    Name = $"ch{chapterIndex}",
                    StartLine = startLine,
                    LengthInLines = counts[chapterIndex],
                });
                startLine += counts[chapterIndex];
            }

            contents.ChapterFiles.Add(new ContentsMapping()
            {
                Name = $"ch{chapterIndex}",
                StartLine = startLine,
                LengthInLines = CountLinesUntilEof(reader) + (startLine > 0 ? 1 : 0),
            });

            BreakInMemory(contents);

            return contents;
        }

        void BreakInMemory(ContentsInfo contents)
        {
            int lineIndex = 0;
            foreach (var chapter in contents.ChapterFiles)
            {
                chapter.PlainTextContent = new List<string>();
                int linesToCopy = chapter.LengthInLines;
                do
                {
                    chapter.PlainTextContent.Add(_chaptersData[lineIndex++]);
                    linesToCopy--;
                }
                while (linesToCopy > 0 && lineIndex < _chaptersData.Count);
            }
            _chaptersData.Clear();
        }

        int CountLinesUntilMarker(TextReader reader, string marker, int i)
        {
            string line = null;
            int count = 0;
            bool hasSkipped = i != 0;
            bool done = false;
            do
            {
                if (!hasSkipped && isLineMarked(marker, line))
                {
                    hasSkipped = true;
                }
                line = reader.ReadLine();

                if (line != null)
                {
                    _chaptersData.Add(line);
                }

                count++;

                done = line == null || hasSkipped && isLineMarked(marker, line);
            }
            while (!done);
            return i == 0 ? count - 1 : count;
        }

        int CountLinesUntilEof(TextReader reader)
        {
            return CountLinesUntilMarker(reader, null, -1);
        }

        bool isLineMarked(string marker, string line) =>
            marker != null && line != null && line.StartsWith(marker, StringComparison.InvariantCulture);
    }
}
