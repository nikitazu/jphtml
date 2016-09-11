using System.Collections.Generic;
using System.Linq;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Core.Format;
using JpAnnotator.Common.Portable.PlainText;

namespace JpAnnotator.Core
{
    public class ContentsBreaker
    {
        readonly IReadOnlyList<string> _chapterMarkers;

        public ContentsBreaker(IOptionProviderChapterMarkers options)
        {
            _chapterMarkers = options.ChapterMarkers;
        }

        public ContentsInfo Analyze(MarkingTextReader reader)
        {
            var contents = new ContentsInfo
            {
                ChapterFiles = new List<ContentsMapping>(_chapterMarkers.Count + 1)
            };

            var counts = _chapterMarkers.Select((marker, i) => reader.CountLinesUntilMarker(marker, i == 0)).ToArray();

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
                LengthInLines = reader.CountLinesUntilEnd() + (startLine > 0 ? 1 : 0),
            });

            BreakInMemory(contents, reader);

            return contents;
        }

        void BreakInMemory(ContentsInfo contents, MarkingTextReader reader)
        {
            int lineIndex = 0;
            foreach (var chapter in contents.ChapterFiles)
            {
                chapter.PlainTextContent = new List<string>();
                int linesToCopy = chapter.LengthInLines;
                do
                {
                    if (lineIndex < reader.Lines.Count)
                    {
                        chapter.PlainTextContent.Add(reader.Lines[lineIndex++]);
                        linesToCopy--;
                    }
                }
                while (linesToCopy > 0 && lineIndex < reader.Lines.Count);
            }
        }
    }
}
