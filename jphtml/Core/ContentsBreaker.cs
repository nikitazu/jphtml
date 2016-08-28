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
        const string _workdir = "tmp";
        readonly IReadOnlyList<string> _chapterMarkers;

        public ContentsBreaker(IReadOnlyList<string> chapterMarkers)
        {
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
                    FilePath = $"{_workdir}/ch{chapterIndex}",
                    StartLine = startLine,
                    LengthInLines = counts[chapterIndex]
                });
                startLine += counts[chapterIndex];
            }

            contents.ChapterFiles.Add(new ContentsMapping()
            {
                FilePath = $"{_workdir}/ch{chapterIndex}",
                StartLine = startLine,
                LengthInLines = CountLinesUntilEof(reader) + (startLine > 0 ? 1 : 0)
            });

            return contents;
        }

        public void Break(string input, ContentsInfo contents)
        {
            if (Directory.Exists(_workdir))
            {
                Directory.Delete(_workdir, true);
            }
            Directory.CreateDirectory(_workdir);

            using (var reader = new StreamReader(input))
            {
                foreach (var chapter in contents.ChapterFiles)
                {
                    int linesToCopy = chapter.LengthInLines;
                    using (var writer = new StreamWriter(chapter.FilePath, false, Encoding.UTF8))
                    {
                        while (linesToCopy > 0 && !reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            writer.WriteLine(line);
                            linesToCopy--;
                        }
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
