using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JpAnnotator.Core.Format;

namespace JpAnnotator.Core
{
    public class ContentsBreaker : IOptionConsumerChapterMarkers
    {
        IReadOnlyList<string> _chapterMarkers;

        public ContentsBreaker(IOptionProvider<IOptionConsumerChapterMarkers> options)
        {
            options.Provide(this);
        }

        void IOptionConsumerChapterMarkers.Consume(IReadOnlyList<string> chapterMarkers)
        {
            _chapterMarkers = chapterMarkers;
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
                    LengthInLines = counts[chapterIndex]
                });
                startLine += counts[chapterIndex];
            }

            contents.ChapterFiles.Add(new ContentsMapping()
            {
                Name = $"ch{chapterIndex}",
                StartLine = startLine,
                LengthInLines = CountLinesUntilEof(reader) + (startLine > 0 ? 1 : 0)
            });

            return contents;
        }

        public void BreakInMemory(string input, ContentsInfo contents)
        {
            using (var reader = new StreamReader(input))
            {
                foreach (var chapter in contents.ChapterFiles)
                {
                    chapter.PlainTextContent = new List<string>();
                    int linesToCopy = chapter.LengthInLines;
                    while (linesToCopy > 0 && !reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        chapter.PlainTextContent.Add(line);
                        linesToCopy--;
                    }
                }
            }
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
