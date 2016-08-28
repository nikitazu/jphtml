using System.IO;
using System.Collections.Generic;
using System.Linq;
using jphtml.Core.Format;

namespace jphtml.Core
{
    public class ContentsBreaker
    {
        readonly IReadOnlyList<string> _chapterMarkers;

        public ContentsBreaker(IReadOnlyList<string> chapterMarkers)
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
                    FilePath = $"tmp/ch{chapterIndex}",
                    StartLine = startLine,
                    LengthInLines = counts[chapterIndex]
                });
                startLine += counts[chapterIndex];
            }

            contents.ChapterFiles.Add(new ContentsMapping()
            {
                FilePath = $"tmp/ch{chapterIndex}",
                StartLine = startLine,
                LengthInLines = CountLinesUntilEof(reader)
            });

            return contents;
        }

        int CountLinesUntilMarker(TextReader reader, string marker, int i)
        {
            string line = null;
            int count = 0;
            bool hasSkipped = i != 0;
            bool done = false;
            do
            {
                if (!hasSkipped && line == marker)
                {
                    hasSkipped = true;
                }
                line = reader.ReadLine();
                count++;

                done = line == null || line == marker && hasSkipped;
            }
            while (!done);
            return i == 0 ? count - 1 : count;
        }

        int CountLinesUntilEof(TextReader reader)
        {
            return CountLinesUntilMarker(reader, null, -1) + 1;
        }
    }
}

