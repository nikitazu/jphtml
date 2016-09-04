using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jphtml.Core.Format;

namespace jphtml.Core
{
    public class ContentsBreaker
    {
        readonly string _outputDir;
        readonly IReadOnlyList<string> _chapterMarkers;

        public ContentsBreaker(string outputDir, IReadOnlyList<string> chapterMarkers)
        {
            _outputDir = outputDir;
            _chapterMarkers = chapterMarkers ?? new List<string>();
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
                    FilePath = $"{_outputDir}/ch{chapterIndex}",
                    StartLine = startLine,
                    LengthInLines = counts[chapterIndex]
                });
                startLine += counts[chapterIndex];
            }

            contents.ChapterFiles.Add(new ContentsMapping()
            {
                FilePath = $"{_outputDir}/ch{chapterIndex}",
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
                    var builder = new StringBuilder();
                    int linesToCopy = chapter.LengthInLines;
                    while (linesToCopy > 0 && !reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        builder.AppendLine(line);
                        linesToCopy--;
                    }
                    chapter.PlainTextContent = builder.ToString();
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
