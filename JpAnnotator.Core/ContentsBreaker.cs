using System.Collections.Generic;
using System.Linq;
using JpAnnotator.Common.Portable.PlainText;
using JpAnnotator.Core.Format;

namespace JpAnnotator.Core
{
    public class ContentsBreaker
    {
        readonly IChapterMarkersProvider _markersProvider;

        public ContentsBreaker(IChapterMarkersProvider markersProvider)
        {
            _markersProvider = markersProvider;
        }

        public ContentsInfo Analyze(MarkingTextReader reader)
        {
            var chapterMarkers = _markersProvider.ProvideChapterMarkers(reader.Lines).ToArray();

            var contents = new ContentsInfo
            {
                ChapterFiles = new List<ContentsMapping>(chapterMarkers.Length + 1)
            };

            int startLine = 0;
            int chapterIndex = 0;
            for (chapterIndex = 0; chapterIndex < chapterMarkers.Length; chapterIndex++)
            {
                var count = reader.CountLinesUntilMarker(chapterMarkers[chapterIndex], chapterIndex == 0);
                contents.ChapterFiles.Add(new ContentsMapping()
                {
                    Name = $"ch{chapterIndex}",
                    StartLine = startLine,
                    LengthInLines = count,
                    PlainTextContent = new List<string>(count),
                });
                Copy(reader.Lines, contents.ChapterFiles[chapterIndex]);
                startLine += count;
            }

            contents.ChapterFiles.Add(new ContentsMapping()
            {
                Name = $"ch{chapterIndex}",
                StartLine = startLine,
                LengthInLines = reader.CountLinesUntilEnd() + (startLine > 0 ? 1 : 0),
                PlainTextContent = new List<string>(),
            });
            Copy(reader.Lines, contents.ChapterFiles[chapterIndex]);

            return contents;
        }

        void Copy(List<string> source, ContentsMapping target)
        {
            for (int i = 0; i < target.LengthInLines && target.StartLine + i < source.Count; i++)
            {
                target.PlainTextContent.Add(source[target.StartLine + i]);
            }
        }
    }
}
